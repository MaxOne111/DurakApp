using System.Collections.Generic;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Responses
{
    public sealed class StatusResponse
        : DurakResponseBase
    {
        [JsonProperty(PropertyName = "status")]
        public EGameStatus Status
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "mode")]
        public ETurnMode Mode
        {
            get;
            set;
        }
        
    }
}