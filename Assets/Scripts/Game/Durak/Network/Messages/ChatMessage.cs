using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Messages
{
    [Serializable]
    public sealed class ChatMessage
        : DurakMessageBase
    {
        [JsonConstructor]
        public ChatMessage(PlayerInfo sender, string text, EChatMessageType type, ETurnMode mode)
            : base(mode)
        {
            Sender = sender;
            Text = text;
            Type = type;
        }
        
        public ChatMessage(PlayerInfo sender, string text, EChatMessageType type)
            : base(ETurnMode.Text)
        {
            Sender = sender;
            Text = text;
            Type = type;
        }

        [JsonProperty(PropertyName = "user")]
        public PlayerInfo Sender
        {
            get;
        }
        
        [JsonProperty(PropertyName = "text")]
        public string Text
        {
            get;
        }
        
        [JsonProperty(PropertyName = "type")]
        public EChatMessageType Type
        {
            get;
        }
    }
}