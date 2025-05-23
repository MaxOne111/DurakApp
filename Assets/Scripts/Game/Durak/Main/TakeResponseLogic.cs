using System.Collections.Generic;
using DG.Tweening;
using Game.Durak;
using Game.Durak.Enums;
using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class TakeResponseLogic : IResponse
{
    private TestPlayer _player; // bind ?

    private List<TestCard> _cardsOnScene;

    private Transform _playerSleeve;

    private GameObject _deck;

    private DurakGameUI _durakGameUI;

    private GameLogicMethods _gameLogicMethods;

    private DurakGameSounds _gameSounds;


    [Inject]
    private TakeResponseLogic(
        List<TestCard> cardsOnScene,
        [Inject(Id = SceneInstallerIdentifiers.PlayerSleevePosition)]
        Transform playerSleeve,
        GameObject deck,
        DurakGameUI durakGameUI,
        GameLogicMethods gameLogicMethods,
        DurakGameSounds gameSounds
        )
    {
        _cardsOnScene = cardsOnScene;
        _playerSleeve = playerSleeve;
        _deck = deck;
        _durakGameUI = durakGameUI;
        _gameLogicMethods = gameLogicMethods;
        _gameSounds = gameSounds;
    }


    public void Invoke(string response)
    {
        TakeResponse takeResponse = JsonConvert.DeserializeObject<TakeResponse>(response);

        if (takeResponse.status != EGameStatus.In_Game)
            return;

        for (int i = 0; i < takeResponse.cards.Length; i++)
        {
            if (_player.HaveCard(takeResponse.cards[i]))
                continue;

            TestCard newCard;

            if (DurakHelper.IsPlayer(_player, takeResponse.taker))
            {
                newCard = DurakHelper.GetCard(_cardsOnScene, takeResponse.cards[i]);
                    
                newCard.transform.SetParent(_playerSleeve);
                newCard.transform.DORotate(Vector3.zero, 0.25f).SetLink(newCard.gameObject);

                _durakGameUI.DisableButton(_durakGameUI.Take);
            }
            else
                newCard = _gameLogicMethods.SpawnCard(takeResponse.cards[i], _deck.transform.position, Quaternion.identity,
                    _playerSleeve);

            _player.AddCard(newCard);
        }

        _gameLogicMethods.ShowPlayersCardCount(takeResponse.players);
            
        Debug.Log($"Refreshing cards amount");
            
        _gameSounds.PlayTakeCard();

        _gameLogicMethods.MoveCardsToSleeve(_player.SleeveCount, 0.5f, 0);
            
        _player.UnlockSleeve();
    }
}