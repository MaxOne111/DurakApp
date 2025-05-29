using System;
using System.Collections.Generic;
using Game.Durak.Core;
using Game.Durak.Main;
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

    private List<TestCard> _cardsOnScene = new List<TestCard>();


    public override void InstallBindings()
    {
        InstallScene();
        InstallPlayer();
        InstallPlayerList();
        InstallCardList();
        InstallServices();
        InstallResponses();
    }

    private void InstallPlayer()
    {
        Container.BindInstance(playerPrefab);

        Container.Bind<Transform>()
            .WithId(SceneInstallerIdentifiers.PlayerSleevePosition)
            .FromInstance(playerSleeve)
            .AsCached();
    }
    
    private void InstallPlayerList()
    {
        Container.Bind<List<TestPlayer>>()
            .WithId(SceneInstallerIdentifiers.PlayersOnScene)
            .FromInstance(_playersOnScene).AsSingle();
    }

    private void InstallCardList()
    {
        Container.Bind<List<TestCard>>()
            .FromInstance(_cardsOnScene)
            .AsSingle();
    }
    
    private void InstallScene()
    {
        Container.Bind<Transform>()
            .WithId(SceneInstallerIdentifiers.TrumpPosition)
            .FromInstance(trumpPosition)
            .AsCached();
        
        Container.Bind<Transform>()
            .WithId(SceneInstallerIdentifiers.BeatPosition)
            .FromInstance(beatPosition)
            .AsCached();

        Container.Bind<GameObject>()
            .FromInstance(deck)
            .AsSingle();

        Container.Bind<EnemyPosition[]>()
            .FromInstance(placesOnTable)
            .AsSingle();

        Container.Bind<Transform>()
            .WithId(SceneInstallerIdentifiers.SlotContainer)
            .FromInstance(slotContainer)
            .AsCached();

        Container.Bind<Image>()
            .FromInstance(trumpImage)
            .AsSingle();

        Container.Bind<TestSlot>()
            .FromInstance(slotPrefab)
            .AsSingle();
    }

    private void InstallServices()
    {
        Container.Bind<DurakGameUI>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<DurakGameSounds>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<ResponseTextMessageRepository>()
            .FromInstance(responseTextMessageRepository)
            .AsCached();

        Container.Bind<CardsConfig>()
            .FromInstance(cardsConfig)
            .AsSingle();

        Container.Bind<GameLogicMethods>()
            .ToSelf()
            .AsSingle();
    }

    private void InstallResponses()
    {
        Container.Bind<JoinResponseLogic>()
            .AsSingle();
        
        Container.Bind<ReadyResponseLogic>()
            .AsSingle();
        Container.Bind<GameStartResponseLogic>()
            .AsSingle();
        
        Container.Bind<RoleResponseLogic>()
            .AsSingle();
        
        Container.Bind<BeatResponseLogic>()
            .AsSingle();
        
        Container.Bind<TakeResponseLogic>()
            .AsSingle();
        
        Container.Bind<IAttackResponse>()
            .To<AttackResponseLogic>()
            .AsSingle();
        
        Container.Bind<DefenceResponseLogic>()
            .AsSingle();
        
        Container.Bind<ChatResponseLogic>()
            .AsSingle();
        
        Container.Bind<ErrorResponseLogic>()
            .AsSingle();

        Container.Bind<InfoResponseLogic>()
            .AsSingle();
        
        Container.Bind<StatusResponseLogic>()
            .AsSingle();
    }
}

[Serializable]
public enum SceneInstallerIdentifiers
{
    PlayerSleevePosition,
    TrumpPosition,
    BeatPosition,
    SlotContainer,
    PlayersOnScene,
}