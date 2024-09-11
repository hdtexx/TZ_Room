using _Project.Scripts.Services.InputService;
using _Project.Scripts.Services.SceneLoader;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI.Gameplay
{
    public class UIGameplayRootView : MonoBehaviour
    {
        [SerializeField] private Button _buttonMainMenu;

        private ISceneLoader _sceneLoader;
        private IInputService _inputService;
        
        [Inject]
        private void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Init(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        private void Update()
        {
            if (_inputService.EscPressed() == true)
            {
                _sceneLoader.LoadMainMenu();
            }
        }
    }
}