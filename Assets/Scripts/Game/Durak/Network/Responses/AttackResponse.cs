using System;
using System.Collections.Generic;
using Game.Durak.Enums;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Game.Durak.Network.Responses
{
    [Serializable]
    public sealed class AttackResponse: DurakResponseBase
    {
        [JsonProperty(PropertyName = "status")]
        public EAttackStatus Status
        {
            get;
            set;
        }
        
        [JsonProperty(PropertyName = "slots")]
        public SlotInfo[] Slots
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