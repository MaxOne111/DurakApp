using Game.Durak.Enums;
using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class BeatResponseLogic : IResponse
{
    private TestPlayer _player; // bind ?
    
    private GameLogicMethods _gameLogicMethods;
    
    private GameObject _deck;
    
    private Transform _playerSleeve;
    
    private DurakGameSounds _gameSounds;


    [Inject]
    private BeatResponseLogic(
        GameLogicMethods gameLogicMethods,
        GameObject deck,
        [Inject(Id = SceneInstallerIdentifiers.PlayerSleevePosition)]
        Transform playerSleeve,
        DurakGameSounds gameSounds)
    {
        _gameLogicMethods = gameLogicMethods;
        _deck = deck;
        _playerSleeve = playerSleeve;
        _gameSounds = gameSounds;
    }
    
    
    public void Invoke(string response)
    {
        BeatResponse beatResponse = JsonConvert.DeserializeObject<BeatResponse>(response);
            
        if (beatResponse.status != EGameStatus.In_Game)
            return;

        if (beatResponse.cards != null)
        {
            for (int i = 0; i < beatResponse.cards.Length; i++)
            {
                if (_player.HaveCard(beatResponse.cards[i]))
                    continue;

                TestCard newCard = _gameLogicMethods.SpawnCard(beatResponse.cards[i], _deck.transform.position, Quaternion.identity,
                    _playerSleeve);

                _player.AddCard(newCard);

                _gameSounds.PlayGetCard();
            }
        }

        if (beatResponse.players != null)
        {
            _gameLogicMethods.ShowPlayersCardCount(beatResponse.players);
        }

        Debug.Log($"Refreshing cards amount");

        _gameLogicMethods.MoveCardsToSleeve(_player.SleeveCount, 0.5f, 50, null);

        _player.UnlockSleeve();
            
    }
}