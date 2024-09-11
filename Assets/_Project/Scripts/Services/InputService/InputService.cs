using UnityEngine;

namespace _Project.Scripts.Services.InputService
{
    public class InputService : IInputService
    {
        public float GetHorizontalInput() => Input.GetAxis("Horizontal");
        
        public float GetVerticalInput() => Input.GetAxis("Vertical");

        public float GetMouseX() => Input.GetAxis("Mouse X");
        
        public float GetMouseY() => Input.GetAxis("Mouse Y");

        public Vector3 GetMousePosition() => Input.mousePosition;
        
        public bool IsLeftMouseButtonPressed() => Input.GetMouseButtonDown(0);

        public bool EscPressed() => Input.GetKeyDown(KeyCode.Escape);
        public float GetMouseScrollInput() => Input.GetAxis("Mouse ScrollWheel");
    }
}