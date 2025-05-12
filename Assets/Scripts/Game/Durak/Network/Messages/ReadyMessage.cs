using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Messages
{
    [Serializable]
    public sealed class ReadyMessage
        : DurakMessageBase
    {
        public ReadyMessage(PlayerInfo info) 
            : base(ETurnMode.Ready)
        {
            user = info;
        }
        
        // [JsonProperty(PropertyName = "user")]
        // public PlayerInfo User
        // {
        //     get;
        // }

        public PlayerInfo user;
    }
}