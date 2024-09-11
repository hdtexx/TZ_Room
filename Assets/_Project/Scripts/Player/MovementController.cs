using _Project.Scripts.Configs;
using _Project.Scripts.Services.InputService;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Project.Scripts.Player
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private Transform _playerCameraTransform;
        private IInputService _inputService;
        private GameplayConfig _gameplayConfig;
        private Vector2 _screenSize;
        private float _xRotation = 0f;
        private float _playerSpeed;
        private float _mouseSensitivity;
        private Vector3 _cameraRestriction = Vector3.zero;

        [Inject]
        private void Construct(IInputService inputService, 
            GameplayConfig gameplayConfig)
        {
            _inputService = inputService;
            _gameplayConfig = gameplayConfig;
            _mouseSensitivity = _gameplayConfig.MouseSensitivity;
            _playerSpeed = _gameplayConfig.PlayerSpeed;
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            MoveCharacter();
            RotateView();
            
            if (_inputService.EscPressed() == true)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
            }
        }

        private void MoveCharacter()
        {
            float horizontal = _inputService.GetHorizontalInput();
            float vertical = _inputService.GetVerticalInput();
            
            Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
            moveDirection = transform.TransformDirection(moveDirection);

            if (moveDirection != Vector3.zero)
            {
                transform.Translate(moveDirection * (_playerSpeed * Time.deltaTime), Space.World);
            }
        }
        
        private void RotateView()
        {
            if (EventSystem.current && EventSystem.current.IsPointerOverGameObject() == true)
            {
                return;
            }

            float mouseX = _inputService.GetMouseX() * _mouseSensitivity * Time.deltaTime;
            float mouseY = _inputService.GetMouseY() * _mouseSensitivity * Time.deltaTime;
    
            if (_cameraRestriction != Vector3.zero)
            {
                if ((_cameraRestriction.y > 0 && mouseY > 0) 
                    || (_cameraRestriction.y < 0 && mouseY < 0))
                {
                    mouseY = 0;
                }
            }

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, 
                _gameplayConfig.MinTopViewAngle, _gameplayConfig.MaxTopViewAngle);
            _playerCameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    
            if (_cameraRestriction != Vector3.zero)
            {
                if ((_cameraRestriction.x > 0 && mouseX > 0) 
                    || (_cameraRestriction.x < 0 && mouseX < 0))
                {
                    mouseX = 0;
                }
            }

            transform.Rotate(Vector3.up * mouseX);
        }

        public void RestrictCameraMovement(Vector3 restriction)
        {
            _cameraRestriction = restriction.normalized;
        }
    }
}