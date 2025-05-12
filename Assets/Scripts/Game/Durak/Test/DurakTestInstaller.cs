using Game.Durak.Core;
using Game.Durak.Test;
using Game.UI;
using UnityEngine;
using Zenject;

namespace Game.Durak.Network
{
    public sealed class DurakTestInstaller
        : MonoInstaller
    {
        [SerializeField] private TestConfigurationScreen configurationScreen;
        [SerializeField] private TestRoomScreen roomScreen;

        [SerializeField] private PlayerView playerPrefab;
        [SerializeField] private Transform playersRoot;

        [SerializeField] private MonoWaitingMarker waitingMarker;
        
        public override void InstallBindings()
        {
            Container.Bind<TestConfigurationScreen>()
                .FromInstance(configurationScreen)
                .AsSingle();
            
            Container.Bind<TestRoomScreen>()
                .FromInstance(roomScreen)
                .AsSingle();

            Container
                .BindInterfacesTo<MonoWaitingMarker>()
                .FromInstance(waitingMarker)
                .AsSingle();

            Container
                .BindInterfacesTo<Core.PlayerFactory>()
                .FromInstance(new Core.PlayerFactory(playerPrefab, playersRoot));

            Container
                .BindInterfacesTo<DurakTestPipeline>()
                .AsSingle();
        }
    }
}