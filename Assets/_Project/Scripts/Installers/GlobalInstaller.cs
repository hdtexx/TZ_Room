using _Project.Scripts.Services.SceneLoader;
using _Project.Scripts.UI.Root;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            
            UIRootView uiRootPrefab = Resources.Load<UIRootView>("UIRoot");
            UIRootView uiRootInstance = Instantiate(uiRootPrefab);
            DontDestroyOnLoad(uiRootInstance);
            Container.Bind<UIRootView>().FromInstance(uiRootInstance).AsSingle();
        }
    }
}
