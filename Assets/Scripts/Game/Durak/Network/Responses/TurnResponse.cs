using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Responses
{
    [Serializable]
    public sealed class TurnResponse
    {
        [JsonProperty(PropertyName = "turn")]
        public EPlayerRole Turn
        {
            get;
            set;
        }
        
        [JsonProperty(PropertyName = "users")]
        public PlayerInfo[] Users
        {
            get;
            set;
        }
    }
}
