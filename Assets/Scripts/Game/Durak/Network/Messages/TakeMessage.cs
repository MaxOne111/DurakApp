using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Messages
{
    [Serializable]
    public sealed class TakeMessage
        : DurakMessageBase
    {
        public TakeMessage(PlayerInfo user) 
            : base(ETurnMode.Take)
        {
            this.user = user;
        }
        
        // [JsonProperty(PropertyName = "user")]
        // public PlayerInfo User
        // {
        //     get;
        // }

        public PlayerInfo user;
    }
}