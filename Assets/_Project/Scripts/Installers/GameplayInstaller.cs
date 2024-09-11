using _Project.Scripts.Configs;
using _Project.Scripts.Player;
using _Project.Scripts.Services.InputService;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInteraction _playerPrefab;
        [SerializeField] private Vector3 _playerStartPosition = new Vector3(0, 1, -3.32f);
        [SerializeField] private GameplayConfig _gameplayConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<IInputService>().To<InputService>().AsSingle();
            
            if (_gameplayConfig)
            {
                Container.Bind<GameplayConfig>().FromInstance(_gameplayConfig).AsSingle();
            }
            else
            {
                GameplayConfig defaultConfig = ScriptableObject.CreateInstance<GameplayConfig>();
                Container.Bind<GameplayConfig>().FromInstance(defaultConfig).AsSingle();
            }
            
            PlayerInteraction player = Container.InstantiatePrefabForComponent<PlayerInteraction>(_playerPrefab);
            Container.Bind<Camera>().FromInstance(player.PlayerCamera).AsSingle();
            player.transform.position = _playerStartPosition;

            MovementController movementController = player.GetComponent<MovementController>();
            
            if (movementController)
            {
                Container.Bind<MovementController>().FromInstance(movementController).AsSingle();
            }
        }
    }
}