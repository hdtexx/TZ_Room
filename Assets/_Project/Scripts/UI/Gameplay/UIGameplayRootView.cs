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
        
        [Inject]
        private void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        private void Awake()
        {
            _buttonMainMenu.onClick.AddListener(OnButtonMainMenuClick);
        }

        private void OnButtonMainMenuClick()
        {
            _sceneLoader.LoadMainMenu();
        }
    }
}