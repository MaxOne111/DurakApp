using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Game.Durak.Network.Responses
{
    [Serializable]
    public sealed class ReadyResponse
        : DurakResponseBase
    {
        // [JsonProperty(PropertyName = "players")]
        // private PlayerInfo[] _players;
        //
        // [JsonProperty(PropertyName = "ready_players")]
        // private PlayerInfo[] _readyPlayers;
        //
        // public IReadOnlyList<PlayerInfo> Players => _players?
        //     .Where(value => value != null)
        //     .ToArray();
        //
        // public IReadOnlyList<PlayerInfo> ReadyPlayers => _readyPlayers?
        //     .Where(value => value != null)
        //     .ToArray();
        
        public PlayerInfo[] players;
        public PlayerInfo[] ready_players;
        public PlayerInfo[] room_players;
        public RoundInfo[] rounds; //not used
        public CardInfo[] beats; //not used
        public PlayerInfo[] queye; //not used
        public PlayerInfo[] active_queye; //not used
        public CardInfo[] deck; //not used
    }
}