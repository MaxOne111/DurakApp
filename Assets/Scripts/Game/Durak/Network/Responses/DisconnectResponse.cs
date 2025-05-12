using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Game.Durak.Network.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class DisconnectResponse
    {
        [JsonProperty(PropertyName = "users")] 
        private PlayerInfo[] _players;
            
        [JsonProperty(PropertyName = "Info")]
        public string Info
        {
            get;
        }

        public IReadOnlyList<PlayerInfo> Players => _players
            .Where(value => value != null)
            .ToArray();
    }
}