using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Messages
{
    [Serializable]
    public sealed class BeatMessage
        : DurakMessageBase
    {
        public BeatMessage(PlayerInfo user) 
            : base(ETurnMode.Beat)
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