using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Responses
{
   // [JsonObject(MemberSerialization.OptIn)]
   [Serializable]
    public sealed class GameStatusResponse
    {
        public GameStatusResponse(EStatusMessage status)
        {
            Status = status;
        }
        
        // [JsonProperty(PropertyName = "Status")]
        // public EStatusMessage Status
        // {
        //     get;
        // }

        public EStatusMessage Status;
    }
}