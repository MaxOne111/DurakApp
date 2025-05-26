using System;
using System.Collections.Generic;
using System.Linq;
using Game.Durak;
using Game.Durak.Core;
using Game.Durak.Enums;
using Game.Durak.Network.Messages;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class ChatResponseLogic : IResponse
{
    private GameLogicMethods _gameLogicMethods;

    private List<TestPlayer> _playersOnScene;


    [Inject]
    private ChatResponseLogic(
        GameLogicMethods gameLogicMethods,
        List<TestPlayer> playersOnScene)
    {
        _gameLogicMethods = gameLogicMethods;
        _playersOnScene = playersOnScene;
    }


    public void Invoke(string response)
    {
        var chatResponse = JsonConvert.DeserializeObject<ChatMessage>(response);

        var prefix = "ActionMessage:";
        if (chatResponse.Text.StartsWith(prefix))
        {
            var mode = Enum.Parse<ETurnMode>(new string(chatResponse.Text.Skip(prefix.Length).ToArray()));
            _gameLogicMethods.ShowActionMessage(chatResponse.Sender, mode, false);
            return;
        }

        var sender = DurakHelper.GetPlayer(_playersOnScene, chatResponse.Sender);

        switch (chatResponse.Type)
        {
            case EChatMessageType.Message:
                var backgroundColor = Color.white;
                var textColor = Color.black;
                    
                var message = new TextMessage(backgroundColor, textColor, chatResponse.Text, 0);
                sender.ShowTextMessage(message);
                break;
                
            case EChatMessageType.Emoji:
                sender.ShowEmoji(chatResponse.Text);
                break;
        }
    }
}