using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Player;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.RoomObjects
{
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] protected VisualSelector _visualSelector;
        [SerializeField] private Rigidbody _rigidbody;

        public ObjectMagnet ObjectMagnet { get; protected set; }
        public ObjectState ObjectState { get; protected set; }
        public bool IsColliding { get; protected set; }
        private MovementController _movementController;
        private Collider[] _objectColliders;
        private Transform _savedParent;
        private float _holdingDistance;
        private bool _isBlocked;
        private int _maxOverlapColliders;

        [Inject]
        private void Construct(GameplayConfig gameplayConfig, MovementController movementController)
        {
            _holdingDistance = gameplayConfig.HoldingDistance;
            _maxOverlapColliders = gameplayConfig.MaxOverlapColliders;
            _movementController = movementController;
        }

        private void Awake()
        {
            _objectColliders = GetComponentsInChildren<Collider>();
            ObjectMagnet = GetComponent<ObjectMagnet>();
            _isBlocked = false;
        }

        private void Update()
        {
            if (ObjectState == ObjectState.PickedUp)
            {
                IsColliding = CheckIsColliding();
            
                if (IsColliding == true && _isBlocked == false)
                {
                    SetBlocked(true);
                }
                else if (IsColliding == false && _isBlocked == true)
                {
                    SetBlocked(false);
                }
            }
        }

        public void UpdateVisualState()
        {
            if (_isBlocked)
            {
                _visualSelector.SetObjectBlockColor();
                return;
            }

            if (ObjectMagnet.IsCurrentlyMagnetized)
            {
                _visualSelector.SetObjectMagnetColor();
            }
            else if (ObjectState == ObjectState.PickedUp)
            {
                _visualSelector.SetTransparencyOnPickUp();
            }
            else
            {
                _visualSelector.ResetObjectColor();
            }
        }

        public void SetCanInteract(bool canInteract)
        {
            _visualSelector.SetCanInteractVisibility(canInteract);
        }

        public void PickUpObject(Transform handle)
        {
            if (ObjectState == ObjectState.PickedUp)
            {
                return;
            }

            SetActiveAllColliders(false);
            _visualSelector.SetCanInteractVisibility(false);
            _savedParent = transform.parent;
            transform.SetParent(handle);
            transform.localPosition = new Vector3(0, 0, _holdingDistance);

            Vector3 forwardToPlayer = -handle.forward;
            Quaternion rotationToPlayer = Quaternion.LookRotation(forwardToPlayer, Vector3.up);
            transform.rotation = Quaternion.Euler(0, rotationToPlayer.eulerAngles.y, 0);

            SetKinematic(true);
            ObjectState = ObjectState.PickedUp;
            UpdateVisualState();
        }

        public void ReleaseObject()
        {
            if (_isBlocked) return;

            SetKinematic(ObjectMagnet.IsCurrentlyMagnetized && ObjectMagnet.StayKinematicAfterMagnet);
            SetActiveAllColliders(true);
            transform.SetParent(_savedParent);
            ObjectMagnet.OnRelease();
            ObjectState = ObjectState.Free;
            UpdateVisualState();
        }

        public void AlignWithSurface(Vector3 surfaceNormal)
        {
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);

            if (ObjectMagnet is ObjectMagnetVertical)
            {
                Vector3 upDirection = Vector3.up;
                targetRotation = Quaternion.LookRotation(-surfaceNormal, upDirection);
            }

            transform.rotation = targetRotation;
        }

        public bool CheckIsColliding()
        {
            Collider[] collisions = GetCollisions();
            bool hasFloorCollider = collisions.Any(col => 
                col.gameObject.layer == LayerMask.NameToLayer("Floor"));

            if (hasFloorCollider == true)
            {
                _movementController.RestrictCameraMovement(-Vector3.up);
            }
            else
            {
                _movementController.RestrictCameraMovement(Vector3.zero);
            }
            
            return collisions.Length > 0;
        }
        
        private Collider[] GetCollisions()
        {
            List<Collider> collidersInContact = new List<Collider>();
            Collider[] results = new Collider[_maxOverlapColliders];

            foreach (Collider col in _objectColliders)
            {
                BoxCollider boxCollider = col as BoxCollider;

                if (!boxCollider)
                {
                    continue;
                }
                
                Vector3 center = boxCollider.center;
                Vector3 size = boxCollider.size;
                Vector3 worldCenter = boxCollider.transform.TransformPoint(center);
                Vector3 worldHalfExtents = Vector3.Scale(size * 0.5f, boxCollider.transform.lossyScale);

                Physics.OverlapBoxNonAlloc(worldCenter, worldHalfExtents, results, boxCollider.transform.rotation);

                foreach (Collider ocol in results)
                {
                    if (ocol != null && ocol != col && ocol.gameObject != col.gameObject 
                        && !collidersInContact.Contains(ocol))
                    {
                        collidersInContact.Add(ocol);
                    }
                }
            }

            return collidersInContact.ToArray();
        }
        
        private void SetBlocked(bool blocked)
        {
            _isBlocked = blocked;

            if (_isBlocked)
            {
                _visualSelector.SetObjectBlockColor();
            }
            else
            {
                UpdateVisualState();
            }
        }
        
        private void SetKinematic(bool isKinematic)
        {
            _rigidbody.isKinematic = isKinematic;
        }

        private void SetActiveAllColliders(bool isActive)
        {
            foreach (Collider col in _objectColliders)
            {
                if (col)
                {
                    col.enabled = isActive;
                }
            }
        }
    }
}