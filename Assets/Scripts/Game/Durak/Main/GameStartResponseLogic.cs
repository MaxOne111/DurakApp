using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class GameStartResponseLogic : IResponse
{
    private GameLogicMethods _gameLogicMethods;
    
    private GameObject _deck;

    private Transform _trumpPosition;


    [Inject]
    private GameStartResponseLogic(
        GameLogicMethods gameLogicMethods,
        GameObject deck,
        [Inject(Id = SceneInstallerIdentifiers.TrumpPosition)]
        Transform trumpPosition)
    {
        _gameLogicMethods = gameLogicMethods;
        _deck = deck;
        _trumpPosition = trumpPosition;
    }
    
    
    public void Invoke(string response)
    {
        GameStartedResponse startedResponse = JsonConvert.DeserializeObject<GameStartedResponse>(response);

        _gameLogicMethods.SpawnSleeve(startedResponse, 0.5f, 100);
            
        _gameLogicMethods.ShowPlayersCardCount(startedResponse?.players);
            
        _deck.SetActive(true);
                
        _trumpPosition.gameObject.SetActive(true);
    }
}