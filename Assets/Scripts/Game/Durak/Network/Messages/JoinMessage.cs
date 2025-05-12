using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Messages
{
    [Serializable]
    public sealed class JoinMessage
        : DurakMessageBase
    {
        public JoinMessage(PlayerInfo info) 
            : base(ETurnMode.Join)
        {
            user = info;
        }

        // [JsonProperty(PropertyName = "user")]
        // public PlayerInfo Info
        // {
        //     get;
        //     set;
        // }

        public PlayerInfo user;
    }
    
    [Serializable]
    public sealed class ReconnectMessage
        : DurakMessageBase
    {
        public ReconnectMessage(PlayerInfo info) 
            : base(ETurnMode.Reconnect)
        {
            user = info;
        }

        // [JsonProperty(PropertyName = "user")]
        // public PlayerInfo Info
        // {
        //     get;
        //     set;
        // }

        public PlayerInfo user;
    }
}