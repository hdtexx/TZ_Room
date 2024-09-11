using System;
using _Project.Scripts.EntryPoints;
using _Project.Scripts.UI.Root;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly UIRootView _uiRoot;
        private int _taskAmount;

        [Inject]
        public SceneLoader(UIRootView uiRoot)
        {
            _uiRoot = uiRoot;
        }

        public void LoadMainMenu()
        {
            LoadAndStartMenuAsync().Forget();
        }

        public void LoadGameplay()
        {
            LoadAndStartGameplayAsync().Forget();
        }

        private async UniTaskVoid LoadAndStartMenuAsync()
        {
            _uiRoot.ResetProgress();
            _uiRoot.ShowLoadingScreen();
            _taskAmount = 2;

            await LoadTask(() => SceneManager.LoadSceneAsync(Scenes.ROOT).ToUniTask(), 1);
            await LoadTask(() => SceneManager.LoadSceneAsync(Scenes.MAIN_MENU).ToUniTask(), 2);

            MainMenuEntryPoint mainMenuEntryPoint = Object.FindObjectOfType<MainMenuEntryPoint>();
            mainMenuEntryPoint.Init(_uiRoot);
            
            await UniTask.Delay(300);
            _uiRoot.HideLoadingScreen();
        }

        private async UniTaskVoid LoadAndStartGameplayAsync()
        {
            _uiRoot.ResetProgress();
            _uiRoot.ShowLoadingScreen();
            _taskAmount = 2;

            await LoadTask(() => SceneManager.LoadSceneAsync(Scenes.ROOT).ToUniTask(), 1);
            await LoadTask(() => SceneManager.LoadSceneAsync(Scenes.GAMEPLAY).ToUniTask(), 2);

            GameplayEntryPoint gameplayEntryPoint = Object.FindObjectOfType<GameplayEntryPoint>();
            gameplayEntryPoint.Init(_uiRoot);
            
            await UniTask.Delay(300);
            _uiRoot.HideLoadingScreen();
        }

        private async UniTask LoadTask(Func<UniTask> task, int taskIndex)
        {
            await task();
            _uiRoot.SetProgress(taskIndex, _taskAmount);
        }
    }
}