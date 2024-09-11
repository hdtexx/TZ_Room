using _Project.Scripts.Services.InputService;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputService>().To<InputService>().AsSingle();
        }
    }
}