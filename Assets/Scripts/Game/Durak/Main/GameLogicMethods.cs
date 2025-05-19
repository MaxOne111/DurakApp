using System;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using Game.Durak;
using Game.Durak.Core;
using Game.Durak.Enums;
using Game.Durak.Network.Responses;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GameLogicMethods
{
    private TestCard _trump;

    private Transform _trumpPosition;
    
    private Transform _beatPosition;
    
    private CardsConfig _cardsConfig;

    private ECardSuit _trumpSuit;

    private Image _trumpImage;

    private DurakGameUI _durakGameUI;

    private DurakGameSounds _gameSounds;

    private GameObject _deck;

    private Transform _playerSleeve;

    private TestPlayer _player;

    private List<TestPlayer> _playersOnScene;

    private List<TestSlot> _slots;

    private List<TestCard> _cardsOnScene;

    private TestPlayer _playerPrefab;

    private EnemyPosition[] _placesOnTable;
    
    private ResponseTextMessageRepository _responseTextMessageRepository;

    private Transform p0;
    private Transform p1;
    private Transform p2;
    private Transform p3;


    [Inject]
    private GameLogicMethods(
        [Inject(Id = SceneInstallerIdentifiers.TrumpPosition)]
        Transform trumpPosition,
        [Inject(Id = SceneInstallerIdentifiers.BeatPosition)]
        Transform beatPosition,
        [Inject(Id = SceneInstallerIdentifiers.PlayerSleevePosition)]
        Transform playerSleeve,
        [Inject(Id = SceneInstallerIdentifiers.PlayersOnScene)]
        List<TestPlayer> playersOnScene,
        CardsConfig cardsConfig,
        Image trumpImage,
        List<TestSlot> slots,
        DurakGameUI durakGameUI,
        DurakGameSounds gameSounds,
        List<TestCard> cardsOnScene,
        GameObject deck,
        TestPlayer playerPrefab,
        EnemyPosition[] placesOnTable,
        ResponseTextMessageRepository messageRepository
    )
    {
        _trumpPosition = trumpPosition;
        _beatPosition = beatPosition;
        _playerSleeve = playerSleeve;
        _playersOnScene = playersOnScene;
        _cardsConfig = cardsConfig;
        _trumpImage = trumpImage;
        _slots = slots;
        _durakGameUI = durakGameUI;
        _gameSounds = gameSounds;
        _cardsOnScene = cardsOnScene;
        _deck = deck;
        _playerPrefab = playerPrefab;
        _placesOnTable = placesOnTable;
        _responseTextMessageRepository = messageRepository;
    }

    public void SpawnSleeve(GameStartedResponse startedResponse, float duration, int delay)
    {
        if (startedResponse.cards.Length == 0)
            return;
            
        _trump = Object.Instantiate(_cardsConfig.GetCard(startedResponse.trump), _trumpPosition);

        ((RectTransform) _trump.transform).sizeDelta = ((RectTransform) _trumpPosition.transform).sizeDelta;
            
        _trumpSuit = _trump.CardInfo.suit;
            
        Sprite trumpSprite = _cardsConfig.GetTrump(_trumpSuit);
            
        _trumpImage.sprite = trumpSprite;
        _trumpImage.gameObject.SetActive(true);
            
        _durakGameUI.SetTrumpSuit(_trumpSuit);

        for (int i = 0; i < startedResponse.cards.Length; i++)
        {
            TestCard card = SpawnCard(startedResponse.cards[i], _deck.transform.position, Quaternion.identity,
                _playerSleeve);
                
            _gameSounds.PlayDistribution();
                
            _player.AddCard(card);
        }

        MoveCardsToSleeve(_player.SleeveCount, duration, delay, _gameSounds.PlayDistribution);
            
        _player.UnlockSleeve();
    }
        
    public void MoveCardsToSleeve(int sleeveCount, float duration, int delay, Action playSound = null, Action callback = null)
    {
        playSound ??= () => { };
        callback ??= () => { };

        Vector3[] points = BezierCurve.GetSleeveCurve(p0, p1, p2, p3, sleeveCount);
            
        _player.MoveCardsToSleeve(points, duration, delay, playSound, callback);
    }
        

    public TestCard SpawnCard(CardInfo info, Transform parent)
    {
        TestCard card = Object.Instantiate(_cardsConfig.GetCard(info), parent);
            
        DurakHelper.CheckTrump(_trumpSuit, card);

        return card;
    }
        
    public TestCard SpawnCard(CardInfo info, Vector3 position, Quaternion rotation, Transform parent)
    {
        TestCard card = Object.Instantiate(_cardsConfig.GetCard(info), position, rotation, parent);
            
        DurakHelper.CheckTrump(_trumpSuit, card);

        return card;
    }
        
    public void MoveCardTo(TestCard card, Vector3 position)
    {
        if (card == null)
            return;

        card.transform.DOLocalMove(position, 0.5f)
            .SetLink(card.gameObject)
            .SetEase(Ease.OutQuad);
            
        card.transform.SetAsLastSibling();
    }
     
    public void ShowPlayersCardCount(PlayerCardInfo[] players)
    {

        foreach (var playerToRefresh in players)
        {
            if (playerToRefresh.cards == 0)
            {
                continue;
            }
                
            if (playerToRefresh.user.user_id == SceneMediator.GetPlayerInfo().user_id)
            {
                continue;
            }

            Debug.Log($"Player: {playerToRefresh.user.username}, cards amount: {playerToRefresh.cards}");
                
            DurakHelper
                .GetPlayer(_playersOnScene, playerToRefresh.user)
                .SetCardAmount(playerToRefresh.cards);
        }
    }

    public void SetCardScale(TestCard card, Vector3 scale)
    {
        card.transform.DOScale(scale, 0.5f)
            .SetLink(card.gameObject);
    }
        
    public void CheckDeck(int count)
    {
        _durakGameUI.ShowCardsCount(count);

        DebugMethod("CheckDeck");
            
        if (count <= 1)
        {
            _deck.SetActive(false);
            Debug.Log("Disable deck");
        }

        if (count <= 0 && _trump)
        {
            Object.Destroy(_trump.gameObject);
            _trumpPosition.gameObject.SetActive(false);
        }
            
    }

    public void CheckBeat(SlotInfo[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].status == false)
                break;
                    
            if (i == slots.Length - 1 && _player.SleeveCount > 0)
                _durakGameUI.SwitchButton(_durakGameUI.Beat);
        }
    }
     
    public void CheckTake(IReadOnlyList<SlotInfo> slots)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].status == false)
            {
                _durakGameUI.SwitchButton(_durakGameUI.Take);
                return;
            }
                    
            _durakGameUI.DisableButtons();
        }
    }

    public void CleanDeck()
    {
        for (int i = 0; i < _slots.Count; i++)
            Object.Destroy(_slots[i].gameObject);

        _cardsOnScene.Clear();
        _slots.Clear();
    }

    public void CleanBeat()
    {
        for (int i = 0; i < _beatPosition.childCount; i++)
            Object.Destroy(_beatPosition.GetChild(i).gameObject);
    }
        
                
    public void MoveCardsToEnemy(PlayerInfo player)
    {
        Debug.Log($"MoveCardsToEnemy: {player.username}");

        var playerView = DurakHelper.GetPlayer(_playersOnScene, player);
        var parent = playerView.transform;
            
        for (int i = 0; i < _cardsOnScene.Count; i++)
        {
            var card = _cardsOnScene[i];
                
            card.FaceDown();
                
            card.transform.SetParent(parent.parent);

            card.transform
                .DOLocalMove(parent.localPosition, 0.25f).SetLink(card.gameObject)
                .OnComplete(() =>
                {
                    card.transform.DOScale(new Vector3(0, 0), 0.25f).SetLink(card.gameObject);

                    // playerView.HideActionMessage();

                });
        }
    }

    public void MoveCardsToBeat()
    {
        Debug.Log($"MoveCardsToBeat");
            
        float[] angles = { -45, 45 };
            
        for (int i = 0; i < _cardsOnScene.Count; i++)
        {
            int index = Random.Range(0, angles.Length);
                
            _cardsOnScene[i].FaceDown();
                
            _cardsOnScene[i].transform.SetParent(_beatPosition);
            _cardsOnScene[i].transform.DOScale(Vector2.one, 0.25f).SetLink(_cardsOnScene[i].gameObject);
            _cardsOnScene[i].transform.DOLocalMove(Vector3.zero, 0.25f).SetLink(_cardsOnScene[i].gameObject);
            _cardsOnScene[i].transform.DORotate(new Vector3(0, 0, angles[index]), 0.25f).SetLink(_cardsOnScene[i].gameObject);
        }
    }
    
    
     public void CreatePlayers(int count, bool isPlayerIncluded = true)
        {
            if (_playersOnScene.Count > 0)
                return;

            PlayerInfo playerInfo = SceneMediator.GetPlayerInfo();
            
            var positions = new EnemyPosition[count];
            
            positions[0] = _placesOnTable[0]; //Player place
            
            switch (count)
            {
                case 2: positions[1] = _placesOnTable[3];
                    break;
                case 3: positions[1] = _placesOnTable[2];
                        positions[2] = _placesOnTable[4];
                    break;
                case 4: positions[1] = _placesOnTable[2];
                        positions[2] = _placesOnTable[3];
                        positions[3] = _placesOnTable[4];
                    break;
                case 5: positions[1] = _placesOnTable[1];
                        positions[2] = _placesOnTable[2];
                        positions[3] = _placesOnTable[4];
                        positions[4] = _placesOnTable[5];
                    break;
                case 6: positions[1] = _placesOnTable[1];
                        positions[2] = _placesOnTable[2];
                        positions[3] = _placesOnTable[3];
                        positions[4] = _placesOnTable[4];
                        positions[5] = _placesOnTable[5];
                    break;
            }
            
            TestPlayer player = Object.Instantiate(_playerPrefab, positions[0].Transform);
            player.Initialize(playerInfo.username, playerInfo.user_id, playerInfo.balance);
            player.ShowDefaultBetHolder(playerInfo.balance.ToString());
            player.SetFlagPosition(EFlagPosition.Left);
            player.SetCardsCountIndicatorEnabled(false);

            //var country = new IpCountryProvider().GetCountry();
            //player.SetCountry(country);
            
            _player = player;
            
            _playersOnScene.Add(player);

            if (!isPlayerIncluded)
                _player.gameObject.SetActive(false);

            int startIndex;

            if (isPlayerIncluded)
                startIndex = 1;
            else
                startIndex = 0;

            for (int i = startIndex; i < positions.Length; i++)
            {
                TestPlayer enemy = Object.Instantiate(_playerPrefab, positions[i].Transform);
                enemy.SetFlagPosition(EFlagPosition.Left);
                enemy.HideBetHolder();
                //enemy.SetFlagPosition(positions[i].FlagPosition);
                //enemy.SetCountry(ECountry.Unknown);
                //enemy.SetAvatar(enemyAvatar);
                
                _playersOnScene.Add(enemy);
            }
        }
     
      public void InitializePlayers(PlayerInfo[] players)
        {
            List<PlayerInfo> playersInfo = new List<PlayerInfo>(players.Length);
            List<PlayerInfo> enemiesAfterPlayer = new List<PlayerInfo>(players.Length);
            List<PlayerInfo> enemiesBeforePlayer = new List<PlayerInfo>(players.Length);
            
            bool playerFound = false;
            
            PlayerInfo playerInfo = SceneMediator.GetPlayerInfo();

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].user_id == 0)
                    continue;
                
                playersInfo.Add(players[i]);
            }
            
            for (int i = 0; i < playersInfo.Count; i++)
            {
                if (playersInfo[i].user_id == playerInfo.user_id)
                {
                    playerFound = true;
                    continue;
                }
                
                if (playerFound)
                    enemiesAfterPlayer.Add(playersInfo[i]);
                else
                    enemiesBeforePlayer.Add(playersInfo[i]);
                
            }
            
            InitializeEnemiesBeforePlayer();
            InitializeEnemiesAfterPlayer();
            

            #region InitializeEnemies

            void InitializeEnemiesBeforePlayer()
            {
                int last = _playersOnScene.Count - 1;
                
                while (enemiesBeforePlayer.Count > 0)
                {
                    if (_playersOnScene[last].PlayerInfo != null)
                    {
                        if (_playersOnScene[last].PlayerInfo.user_id == playerInfo.user_id)
                        {
                            last--;
                            continue;
                        }
                    }
                    
                    _playersOnScene[last].Initialize(enemiesBeforePlayer[^1].username, enemiesBeforePlayer[^1].user_id, enemiesBeforePlayer[^1].balance);

                    enemiesBeforePlayer.Remove(enemiesBeforePlayer[^1]);

                    last--;
                }
            }
            
            
            void InitializeEnemiesAfterPlayer()
            {
                int first = 0;
                
                while (enemiesAfterPlayer.Count > 0)
                {
                    if (_playersOnScene[first].PlayerInfo != null)
                    {
                        if (_playersOnScene[first].PlayerInfo.user_id == playerInfo.user_id)
                        {
                            first++;
                            continue;
                        }
                    }
                
                    _playersOnScene[first].Initialize(enemiesAfterPlayer[0].username, enemiesAfterPlayer[0].user_id, enemiesAfterPlayer[0].balance);

                    enemiesAfterPlayer.Remove(enemiesAfterPlayer[0]);

                    first++;
                }
            }

            #endregion
        }
      
      
      public void InitializePlayers(PlayerCardInfo[] players)
        {
            List<PlayerInfo> playersInfo = new List<PlayerInfo>(players.Length);
            List<PlayerInfo> enemiesAfterPlayer = new List<PlayerInfo>(players.Length);
            List<PlayerInfo> enemiesBeforePlayer = new List<PlayerInfo>(players.Length);
            
            bool playerFound = false;
            
            PlayerInfo playerInfo = SceneMediator.GetPlayerInfo();

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].user.user_id == 0)
                    continue;
                
                playersInfo.Add(players[i].user);
            }

            for (int i = 0; i < playersInfo.Count; i++)
            {
                if (playersInfo[i].user_id == playerInfo.user_id)
                {
                    playerFound = true;
                    continue;
                }
                
                if (playerFound)
                    enemiesAfterPlayer.Add(playersInfo[i]);
                else
                    enemiesBeforePlayer.Add(playersInfo[i]);
                
            }

            InitializeEnemiesBeforePlayer();
            InitializeEnemiesAfterPlayer();
            Debug.Log("Point 4");

            #region InitializeEnemies

            void InitializeEnemiesBeforePlayer()
            {
                int last = _playersOnScene.Count - 1;
                
                while (enemiesBeforePlayer.Count > 0)
                {
                    if (_playersOnScene[last].PlayerInfo != null)
                    {
                        if (_playersOnScene[last].PlayerInfo.user_id == playerInfo.user_id)
                        {
                            last--;
                            continue;
                        }
                    }
                    
                    _playersOnScene[last].Initialize(enemiesBeforePlayer[^1].username, enemiesBeforePlayer[^1].user_id, enemiesBeforePlayer[^1].balance);

                    enemiesBeforePlayer.Remove(enemiesBeforePlayer[^1]);

                    last--;
                }
            }
            
            
            void InitializeEnemiesAfterPlayer()
            {
                int first = 0;
                
                while (enemiesAfterPlayer.Count > 0)
                {
                    if (_playersOnScene[first].PlayerInfo != null)
                    {
                        if (_playersOnScene[first].PlayerInfo.user_id == playerInfo.user_id)
                        {
                            first++;
                            continue;
                        }
                    }
                
                    _playersOnScene[first].Initialize(enemiesAfterPlayer[0].username, enemiesAfterPlayer[0].user_id, enemiesAfterPlayer[0].balance);

                    enemiesAfterPlayer.Remove(enemiesAfterPlayer[0]);

                    first++;
                }
            }

            #endregion
        }
      
      
      public void ShowActionMessage(PlayerInfo user, ETurnMode mode, bool sendToServer = false)
      {
          Debug.Log($"ShowActionMessage: username {user.username}, mode: {mode}");
            
          var player = DurakHelper.GetPlayer(_playersOnScene, user);

          switch (mode)
          {
              case ETurnMode.Beat:
                  if (player.PlayerInfo.user_id == _player.PlayerInfo.user_id)
                  {
                      _durakGameUI.DisableButton(_durakGameUI.Beat);
                  }
                  break;
                
              case ETurnMode.Pass:
                  if (player.PlayerInfo.user_id == _player.PlayerInfo.user_id)
                  {
                      _durakGameUI.DisableButton(_durakGameUI.Pass);
                  }
                  break;
                
              case ETurnMode.Take:
                  if (player.PlayerInfo.user_id == _player.PlayerInfo.user_id)
                  {
                      _durakGameUI.DisableButton(_durakGameUI.Take);
                  }
                  break;
          }

          var hasMessage = _responseTextMessageRepository.TryGetMessage(mode, out var message);
          if (hasMessage)
          {
              player.ShowActionMessage(message);

              if (sendToServer)
              {
                 // SendTextMessage($"ActionMessage:{mode}");
              }
          }
      }
    
        
    public void DebugMethod(string methodName)
    {
        Debug.Log($"----------Now: {methodName}----------[{DateTime.Now.TimeOfDay}]");
    }
        
    public void DebugMethod(MethodInfo method, string response)
    {
        Debug.Log($"----------Now: {method.Name}----------[{DateTime.Now.TimeOfDay}]");
        Debug.Log($"Message received: {JObject.Parse(response)}----------[{DateTime.Now.TimeOfDay}]");
    }
}