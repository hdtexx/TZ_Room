using _Project.Scripts.Configs;
using _Project.Scripts.Services.InputService;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.RoomObjects
{
    public class ObjectMagnetVertical : ObjectMagnet
    {
        private Vector3 _currentNormal;
        private Vector3 _magnetPosition;
        private float _holdingDistance;
        private float _magnetSurfaceOffset;
        private float _currentRotationAngle;

        [Inject]
        protected override void Construct(Camera playerCamera, GameplayConfig gameplayConfig,
            IInputService inputService)
        {
            base.Construct(playerCamera, gameplayConfig, inputService);
            _holdingDistance = gameplayConfig.HoldingDistance;
            _magnetSurfaceOffset = gameplayConfig.MagnetVerticalSurfaceOffset;
        }

        protected override void Update()
        {
            base.Update();

            if (_interactableObject.ObjectState == ObjectState.PickedUp)
            {
                if (IsCurrentlyMagnetized)
                {
                    HandleScrollRotation();
                }
            }
        }

        private void HandleScrollRotation()
        {
            float scrollInput = _inputService.GetMouseScrollInput();

            if (scrollInput > 0)
            {
                _currentRotationAngle += _rotationAngle;
            }
            else if (scrollInput < 0)
            {
                _currentRotationAngle -= _rotationAngle;
            }

            Quaternion rotationAroundNormal = Quaternion.AngleAxis(_currentRotationAngle, _currentNormal);
            _interactableObject.transform.rotation = rotationAroundNormal * _interactableObject.transform.rotation;
        }

        public override void HandleMagnet()
        {
            Ray ray = new Ray(_playerCameraTransform.position, _playerCameraTransform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, _maxMagnetDistance, _placableLayerMask))
            {
                IPlacable placableSurface = hit.collider.GetComponent<IPlacable>();

                if (placableSurface != null)
                {
                    Vector3 hitPoint = hit.point;
                    Vector3 hitNormal = hit.normal;

                    _magnetPosition = hitPoint;
                    _currentNormal = hitNormal;

                    Quaternion targetRotation = Quaternion.LookRotation(-hitNormal, Vector3.up);

                    transform.position = hitPoint + hitNormal * _magnetSurfaceOffset;
                    transform.rotation = targetRotation * Quaternion.Euler(90f, 0f, 0f);

                    if (!IsCurrentlyMagnetized)
                    {
                        IsCurrentlyMagnetized = true;
                        _interactableObject.UpdateVisualState();
                    }

                    return;
                }
            }

            if (IsCurrentlyMagnetized)
            {
                IsCurrentlyMagnetized = false;
                _interactableObject.UpdateVisualState();
                transform.localPosition = new Vector3(0, 0, _holdingDistance);
            }
        }
    }
}