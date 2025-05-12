using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Messages
{
    [Serializable]
    public sealed class PassMessage
        : DurakMessageBase
    {
        public PassMessage(PlayerInfo user) 
            : base(ETurnMode.Pass)
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