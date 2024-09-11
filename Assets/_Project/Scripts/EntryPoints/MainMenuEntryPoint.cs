using _Project.Scripts.Services.InputService;
using _Project.Scripts.UI.Menu;
using _Project.Scripts.UI.Root;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.EntryPoints
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIMainMenuRootView _uiMainMenuRootViewPrefab;
        
        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Init(UIRootView uiRootView)
        {
            UIMainMenuRootView sceneUI = ProjectContext.Instance.Container
                .InstantiatePrefabForComponent<UIMainMenuRootView>(_uiMainMenuRootViewPrefab);
            uiRootView.AttachSceneUI(sceneUI.gameObject);
        }
    }
}