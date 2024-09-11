using _Project.Scripts.Services.InputService;
using _Project.Scripts.UI.Gameplay;
using _Project.Scripts.UI.Root;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.EntryPoints
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIGameplayRootView _uIGameplayRootViewPrefab;
        
        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        public void Init(UIRootView uiRootView)
        {
            UIGameplayRootView sceneUI = ProjectContext.Instance.Container
                .InstantiatePrefabForComponent<UIGameplayRootView>(_uIGameplayRootViewPrefab);
            uiRootView.AttachSceneUI(sceneUI.gameObject);
        }
    }
}