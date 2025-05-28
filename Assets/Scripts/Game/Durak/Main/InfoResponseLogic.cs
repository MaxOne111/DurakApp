using System.Collections.Generic;
using Game.Durak;
using Game.Durak.Enums;
using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class InfoResponseLogic : IResponse
{
    private GameLogicMethods _gameLogicMethods;

    private DurakGameUI _durakGameUI;

    private DurakGameSounds _gameSounds;

    private List<TestPlayer> _playersOnScene;

    private CardsConfig _cardsConfig;

    private TestPlayer _player; // Bind ?
    
    private TestPlayer _activePlayer; // Bind ?

    private bool _inGame; // Temp
    
    private bool _isReady; // Temp


    [Inject]
    private InfoResponseLogic(
        GameLogicMethods gameLogicMethods,
        DurakGameUI durakGameUI,
        DurakGameSounds gameSounds,
        List<TestPlayer> playersOnScene,
        CardsConfig cardsConfig)
    {
        _gameLogicMethods = gameLogicMethods;
        _durakGameUI = durakGameUI;
        _gameSounds = gameSounds;
        _playersOnScene = playersOnScene;
        _cardsConfig = cardsConfig;
    }
    
    
    public void Invoke(string response)
    {
        InfoResponse infoResponse = JsonConvert.DeserializeObject<InfoResponse>(response);

        TestPlayer player;

        PlayerInfo playerInfo = SceneMediator.GetPlayerInfo();
            
        switch (infoResponse?.Info)
        {
            case ELogicInfo.DeckSize:
                _gameLogicMethods.CheckDeck(int.Parse(infoResponse.value));
                    
                break;

            case ELogicInfo.CanPass:

                break;
                
            case ELogicInfo.DefenderTake:
                _gameLogicMethods.ShowActionMessage(infoResponse.user, infoResponse.action);

                break;
                
            case ELogicInfo.DefenderTook:
                Debug.Log($"Defender took the cards: {infoResponse.user.username}");
                    
                if (!DurakHelper.IsPlayer(_player, infoResponse.user))
                    _gameLogicMethods.MoveCardsToEnemy(infoResponse.user);
                    
                break;
                
            case ELogicInfo.AttackerSkipped:
                _gameLogicMethods.ShowActionMessage(infoResponse.user, infoResponse.action);
                Debug.Log($"AttackerSkipped: user: {infoResponse.user}, mode: {infoResponse.action}");
                break;
                
            case ELogicInfo.InitSkipped:
                _gameLogicMethods.ShowActionMessage(infoResponse.user, infoResponse.action);
                Debug.Log($"InitSkipped: user: {infoResponse.user}, mode: {infoResponse.action}");
                break;
                
            case ELogicInfo.YourTurn:
                if (infoResponse.action == ETurnMode.Beat)
                    _durakGameUI.SwitchButton(_durakGameUI.Beat);
                    
                else if (infoResponse.action == ETurnMode.Pass)
                    _durakGameUI.SwitchButton(_durakGameUI.Pass);
                break;
                
            case ELogicInfo.PlayerWon:
                player = DurakHelper.GetPlayer(_playersOnScene, infoResponse.user);
                player.DarkenAvatar();
                player.DisableFrames();

                if (DurakHelper.IsPlayer(_player, player))
                {
                    _gameSounds.PlayVictory();
                    _durakGameUI.DisableButtons();
                }
                    
                break;
                
            case ELogicInfo.PlayerLose:
                player = DurakHelper.GetPlayer(_playersOnScene, infoResponse.user);
                player.DisableFrames();

                if (DurakHelper.IsPlayer(_player, player))
                {
                    _gameSounds.PlayDefeat();
                    _durakGameUI.DisableButtons();
                }

                break;
                
            case ELogicInfo.NewRound:

                if (infoResponse.action is ETurnMode.Beat)
                    _gameLogicMethods.MoveCardsToBeat();

                for (int i = 0; i < _playersOnScene.Count; i++)
                    _playersOnScene[i].HideActionMessage();
                    
                _gameLogicMethods.CleanDeck();
                break;
                
            case ELogicInfo.UserLogOut:
                _isReady = false;

                _durakGameUI.DisableButton(_durakGameUI.Ready);
                    
                player = DurakHelper.GetPlayer(_playersOnScene, infoResponse.user);

                if (!_inGame)
                {
                    for (int i = 0; i < _playersOnScene.Count; i++)
                        _playersOnScene[i].StopTimer();
                }
                    
                if (player.PlayerInfo.user_id == playerInfo.user_id)
                {
                    _gameLogicMethods.Quit();
                    break;
                }
                    
                player.DarkenAvatar();
                player.ShowDisconnectMessage();

                break;
                
            case ELogicInfo.UserReconnect:
                player = DurakHelper.GetPlayer(_playersOnScene, infoResponse.user);

                if (DurakHelper.IsPlayer(_player, player))
                    break;
                    
                player.LightenAvatar();
                player.HideDisconnectMessage();

                break;
                
            case ELogicInfo.ActivePlayer:
                _activePlayer = DurakHelper.GetPlayer(_playersOnScene, infoResponse.user);
                _activePlayer.EnableAttackFrame();

                _activePlayer.HideActionMessage();

                if (DurakHelper.IsPlayer(_player, _activePlayer))
                    _gameLogicMethods.Vibrate();

                break;
                
            case ELogicInfo.MinTrumpCard:

                player = DurakHelper.GetPlayer(_playersOnScene, infoResponse.user);

                TestCard card = _cardsConfig.GetCard(infoResponse.user.min_trump_card);
                    
                _durakGameUI.SetTrumpSuit(card.CardInfo.suit);
                    
                if (DurakHelper.IsPlayer(_player, player))
                    break;
                    
                if(player)
                    player.ShowMinTrump(card);
                    
                break;
                
            case ELogicInfo.ActiveReady:
                    
                Debug.Log(_playersOnScene.Count);
                    
                for (int i = 0; i < _playersOnScene.Count; i++)
                    _playersOnScene[i].EnableAttackFrame();
                    
                break;
        }
    }
}