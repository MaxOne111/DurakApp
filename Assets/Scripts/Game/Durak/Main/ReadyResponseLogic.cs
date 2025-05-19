using System.Collections.Generic;
using Game.Durak;
using Game.Durak.Enums;
using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class ReadyResponseLogic : IResponse
{
    private GameLogicMethods _gameLogicMethods;
    
    private List<TestPlayer> _playersOnScene;


    [Inject]
    private ReadyResponseLogic(
        GameLogicMethods gameLogicMethods,
        [Inject(Id = SceneInstallerIdentifiers.PlayersOnScene)]
        List<TestPlayer> playersOnScene)
    {
        _gameLogicMethods = gameLogicMethods;
        _playersOnScene = playersOnScene;
    }
    
    
    public void Invoke(string response)
    {
        ReadyResponse readyResponse = JsonConvert.DeserializeObject<ReadyResponse>(response);

        for (int i = 0; i < readyResponse.ready_players.Length; i++)
        {
            var player = DurakHelper.GetPlayer(_playersOnScene, readyResponse.ready_players[i]);

            _gameLogicMethods.ShowActionMessage(player.PlayerInfo, ETurnMode.Ready);
            player.DisableFrames();
            player.StopTimer();
        }

        if (readyResponse.ready_players.Length == _playersOnScene.Count)
        {
            Debug.Log("Start game");
        }
    }
    
}