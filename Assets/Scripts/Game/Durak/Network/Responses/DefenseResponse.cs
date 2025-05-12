using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Responses
{
    [Serializable]
    public sealed class DefenseResponse
        : DurakResponseBase
    {
        [JsonProperty(PropertyName = "slots")]
        public SlotInfo[] Slots
        {
            get;
            set;
        }
        
        [JsonProperty(PropertyName = "slot")]
        public int SlotNumber
        {
            get;
            set;
        }
        
        [JsonProperty(PropertyName = "players")]
        public PlayerCardInfo[] Players
        {
            get;
            set;
        }

    }
}