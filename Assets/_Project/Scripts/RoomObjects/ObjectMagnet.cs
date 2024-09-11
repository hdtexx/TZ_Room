using _Project.Scripts.Configs;
using _Project.Scripts.Services.InputService;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.RoomObjects
{
    public abstract class ObjectMagnet : MonoBehaviour
    {
        [SerializeField] protected LayerMask _placableLayerMask;
        [field: SerializeField] public bool StayKinematicAfterMagnet { get; protected set; }
        
        public bool IsCurrentlyMagnetized { get; protected set; }
        
        protected InteractableObject _interactableObject;
        protected Transform _playerCameraTransform;
        protected IInputService _inputService;
        protected float _maxMagnetDistance;
        protected float _rotationAngle;
        
        [Inject]
        protected virtual void Construct(Camera playerCamera, GameplayConfig gameplayConfig,
            IInputService inputService)
        {
            _playerCameraTransform = playerCamera.transform;
            _maxMagnetDistance = gameplayConfig.MaxMagnetDistance;
            _inputService = inputService;
            _rotationAngle = gameplayConfig.RotationAngle;
        }
        
        protected virtual void Awake()
        {
            _interactableObject = GetComponent<InteractableObject>();
        }

        protected virtual void Update()
        {
            if (_interactableObject.ObjectState == ObjectState.PickedUp)
            {
                HandleMagnet();
            }
        }

        public virtual void OnRelease()
        {
            IsCurrentlyMagnetized = false;
            _interactableObject.UpdateVisualState();
        }
        
        public abstract void HandleMagnet();
    }
}