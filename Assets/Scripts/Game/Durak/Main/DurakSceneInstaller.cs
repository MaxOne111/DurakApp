using System;
using System.Collections.Generic;
using Game.Durak.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DurakSceneInstaller : MonoInstaller
{
    [Header("Scene objects")]
    
    [SerializeField] private GameObject deck;
        
    [SerializeField] private Transform playerSleeve;
        
    [SerializeField] private Transform trumpPosition;
        
    [SerializeField] private Transform beatPosition;
        
    [SerializeField] private EnemyPosition[] placesOnTable;
        
    [SerializeField] private Transform slotContainer;

    [SerializeField] private Image trumpImage;
    
    
    [Header("Services/Configs")]

    [SerializeField] private ResponseTextMessageRepository responseTextMessageRepository;
    
    [SerializeField] private CardsConfig cardsConfig;
    
    
    [Header("Prefabs")]
    
    [SerializeField] private TestPlayer playerPrefab;
    
    [SerializeField] private TestSlot slotPrefab;
    

    private List<TestPlayer> _playersOnScene = new List<TestPlayer>();

    private GameLogicMethods _gameLogic = new GameLogicMethods();

    public override void InstallBindings()
    {
        InstallScene();
        InstallPlayer();
        InstallPlayerList();
        InstallServices();
        InstallResponses();
    }

    private void InstallPlayer()
    {
        Container.BindInstance(playerPrefab);

        Container.Bind<Transform>().WithId(SceneInstallerIdentifiers.PlayerSleevePosition).FromInstance(playerSleeve)
            .AsCached();
    }
    
    private void InstallPlayerList()
    {
        Container.Bind<List<TestPlayer>>().FromInstance(_playersOnScene).AsSingle();
    }
    
    private void InstallScene()
    {
        Container.Bind<Transform>().WithId(SceneInstallerIdentifiers.TrumpPosition).FromInstance(trumpPosition)
            .AsCached();
        Container.Bind<Transform>().WithId(SceneInstallerIdentifiers.BeatPosition).FromInstance(beatPosition)
            .AsCached();

        Container.Bind<GameObject>().FromInstance(deck).AsSingle();

        Container.Bind<EnemyPosition[]>().FromInstance(placesOnTable).AsSingle();

        Container.Bind<Transform>().WithId(SceneInstallerIdentifiers.SlotContainer).FromInstance(slotContainer)
            .AsCached();

        Container.Bind<Image>().FromInstance(trumpImage).AsSingle();

        Container.BindInstance(slotPrefab);
    }

    private void InstallServices()
    {
        Container.Bind<DurakGameUI>().FromComponentInHierarchy().AsSingle();

        Container.Bind<DurakGameSounds>().FromComponentInHierarchy().AsSingle();

        Container.Bind<ResponseTextMessageRepository>().FromInstance(responseTextMessageRepository)
            .AsCached();

        Container.Bind<CardsConfig>().FromInstance(cardsConfig).AsSingle();

        Container.Bind<GameLogicMethods>().FromInstance(_gameLogic).AsSingle();
    }

    private void InstallResponses()
    {
        Container.Bind<IAttackResponse>().To<AttackResponseLogic>().AsSingle();
    }
}

[Serializable]
public enum SceneInstallerIdentifiers
{
    PlayerSleevePosition,
    TrumpPosition,
    BeatPosition,
    SlotContainer,
}