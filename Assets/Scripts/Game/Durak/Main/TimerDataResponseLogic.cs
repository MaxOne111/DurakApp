using System;
using System.Collections.Generic;
using Game.Durak.Enums;
using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class TimerDataResponseLogic : IResponse
{
    private List<TestPlayer> _playersOnScene;

    private TestPlayer _activePlayer; // Bind ?


    [Inject]
    private TimerDataResponseLogic(List<TestPlayer> playersOnScene)
    {
        _playersOnScene = playersOnScene;
    }


    public void Invoke(string response)
    {
        TimerDataResponse timerDataResponse = JsonConvert.DeserializeObject<TimerDataResponse>(response);
            
        DateTime currentTime = DateTime.Parse(timerDataResponse.timer.now);
        DateTime finishTime = DateTime.Parse(timerDataResponse.timer.expiration_time);
            
        int seconds = finishTime.Subtract(currentTime).Seconds;
        int minutes = finishTime.Subtract(currentTime).Minutes;

        int timerDuration = seconds + minutes * 60;

        switch (timerDataResponse.mode)
        {
            case ETurnMode.TimerGame:
                _activePlayer.StopTimer();
                _activePlayer.StartTimer(timerDuration);
                    
                break;
                
            case ETurnMode.TimerReady:
                Debug.Log(_playersOnScene.Count);
                for (int i = 0; i < _playersOnScene.Count; i++)
                    _playersOnScene[i].StartTimer(timerDuration);

                break;
        }
    }
}