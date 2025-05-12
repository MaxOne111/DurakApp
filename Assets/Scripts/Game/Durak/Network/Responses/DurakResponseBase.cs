using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Responses
{
    [Serializable]
    public abstract class DurakResponseBase
    {
        // [JsonProperty(PropertyName = "status")]
        // public EGameStatus Status
        // {
        //     get;
        //     set;
        // }
        //
        // [JsonProperty(PropertyName = "mode")]
        // public EResponseMode Mode
        // {
        //     get;
        //     set;
        // }

        public ETurnMode mode;
        public EGameStatus status;
    }
}