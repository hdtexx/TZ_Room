using _Project.Scripts.Configs;
using _Project.Scripts.Services.InputService;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.RoomObjects
{
    public class ObjectMagnetHorizontal : ObjectMagnet
    {
        [SerializeField] private Transform _parentPoint;
        [SerializeField] private float _angleThreshold = 30f;
        private float _magnetSurfaceOffset = 0.01f;
        private float _currentRotationY;

        [Inject]
        protected override void Construct(Camera playerCamera, GameplayConfig gameplayConfig,
            IInputService inputService)
        {
            base.Construct(playerCamera, gameplayConfig, inputService);
            _magnetSurfaceOffset = gameplayConfig.MagnetHorizontalSurfaceOffset;
        }
        
        protected override void Update()
        {
            base.Update();

            if (_interactableObject.ObjectState == ObjectState.PickedUp)
            {
                if (IsCurrentlyMagnetized)
                {
                    RotateWithCamera();
                    HandleScrollRotation();
                }
            }
        }

        private void RotateWithCamera()
        {
            Vector3 cameraForward = _playerCameraTransform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Quaternion rotationToPlayer = Quaternion.LookRotation(cameraForward, Vector3.up);
            _interactableObject.transform.rotation = rotationToPlayer;
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

            Quaternion rotation = Quaternion.Euler(0, _currentRotationY, 0);
            _interactableObject.transform.rotation = rotation * _interactableObject.transform.rotation;
        }

        public override void HandleMagnet()
        {
            Ray ray = new Ray(_playerCameraTransform.position, _playerCameraTransform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, _maxMagnetDistance, _placableLayerMask))
            {
                IPlacable placableSurface = hit.collider.GetComponent<IPlacable>();

                if (placableSurface != null)
                {
                    Vector3 topSurfacePosition = hit.collider.bounds.center + new Vector3(0, hit.collider.bounds.extents.y, 0);
                    Vector3 topNormal = Vector3.up;
                    float angle = Vector3.Angle(topNormal, Vector3.up);

                    if (angle < _angleThreshold)
                    {
                        float offset = topSurfacePosition.y - _parentPoint.position.y + _magnetSurfaceOffset;
                        Vector3 targetPosition = transform.position + Vector3.up * offset;
                        transform.position = targetPosition;

                        _interactableObject.AlignWithSurface(topNormal);

                        if (!IsCurrentlyMagnetized)
                        {
                            IsCurrentlyMagnetized = true;
                            _interactableObject.UpdateVisualState();
                        }

                        return;
                    }
                }
            }

            if (IsCurrentlyMagnetized)
            {
                IsCurrentlyMagnetized = false;
                _interactableObject.UpdateVisualState();
            }
        }
    }
}