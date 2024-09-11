using _Project.Scripts.Configs;
using _Project.Scripts.RoomObjects;
using _Project.Scripts.Services.InputService;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [field: SerializeField] public Camera PlayerCamera { get; private set; }

        private IInputService _inputService;
        private GameplayConfig _gameplayConfig;
        private InteractableObject _currentInteractable;
        private float _interactionDistance;
        private bool _holdingObject;
        private float _rotationAngle;
        private float _currentRotationY;

        [Inject]
        private void Construct(GameplayConfig gameplayConfig, IInputService inputService)
        {
            _gameplayConfig = gameplayConfig;
            _inputService = inputService;
            _interactionDistance = _gameplayConfig.InteractionDistance;
            _rotationAngle = _gameplayConfig.RotationAngle;
        }

        private void Update()
        {
            if (!_holdingObject)
            {
                if (CheckObjectIsAvailabile() == true
                    && _inputService.IsLeftMouseButtonPressed() == true
                    && _currentInteractable)
                {
                    _currentInteractable.PickUpObject(PlayerCamera.transform);
                    _currentInteractable.transform.SetParent(PlayerCamera.transform);
                    AlignObjectWithSurface();
                    RotateObjectToPlayer();
                    _holdingObject = true;
                }
            }
            else
            {
                if (_currentInteractable)
                {
                    if (_inputService.IsLeftMouseButtonPressed() == true)
                    {
                        if (_currentInteractable.CheckIsColliding() == false)
                        {
                            _currentInteractable.ReleaseObject();
                            _currentInteractable.ObjectMagnet.OnRelease();
                            _currentInteractable = null;
                            _holdingObject = false;
                        }

                        return;
                    }

                    if (_currentInteractable.ObjectMagnet.IsCurrentlyMagnetized == false)
                    {
                        RotateWithCamera();
                        HandleScrollRotation();
                    }
                }
            }
        }

        private void HandleScrollRotation()
        {
            float scrollInput = _inputService.GetMouseScrollInput();

            if (scrollInput > 0)
            {
                _currentRotationY += _rotationAngle;
            }
            else if (scrollInput < 0)
            {
                _currentRotationY -= _rotationAngle;
            }

            Vector3 currentRotation = _currentInteractable.transform.rotation.eulerAngles;
            _currentInteractable.transform.rotation = Quaternion.Euler(0, _currentRotationY, 0) *
                                                      Quaternion.Euler(0, currentRotation.y, 0);
        }

        private void RotateWithCamera()
        {
            Vector3 cameraForward = PlayerCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Quaternion rotationToPlayer = Quaternion.LookRotation(cameraForward, Vector3.up);
            _currentInteractable.transform.rotation = rotationToPlayer;
        }

        private void RotateObjectToPlayer()
        {
            Vector3 directionToPlayer = PlayerCamera.transform.forward;
            directionToPlayer.y = 0;
            directionToPlayer.Normalize();

            Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            _currentInteractable.transform.rotation = rotationToPlayer;
        }

        private void AlignObjectWithSurface()
        {
            _currentInteractable.AlignWithSurface(Vector3.up);
        }

        private bool CheckObjectIsAvailabile()
        {
            Ray ray = PlayerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));

            if (Physics.Raycast(ray, out var hit, _interactionDistance))
            {
                InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();

                if (interactable)
                {
                    if (_currentInteractable != interactable)
                    {
                        if (_currentInteractable)
                        {
                            _currentInteractable.SetCanInteract(false);
                        }

                        _currentInteractable = interactable;
                        _currentInteractable.SetCanInteract(true);
                    }
                }
                else
                {
                    if (_currentInteractable)
                    {
                        _currentInteractable.SetCanInteract(false);
                        _currentInteractable = null;
                    }
                }

                return true;
            }

            if (_currentInteractable)
            {
                _currentInteractable.SetCanInteract(false);
                _currentInteractable = null;
            }

            return false;
        }
    }
}