using UnityEngine;

namespace _Project.Scripts.Services.InputService
{
    public interface IInputService
    {
        public float GetHorizontalInput();
        public float GetVerticalInput();
        public float GetMouseX();
        public float GetMouseY();
        public Vector3 GetMousePosition();
        public bool IsLeftMouseButtonPressed();
        public bool EscPressed();
        public float GetMouseScrollInput();
    }
}