using System;
using System.Collections.Generic;
using System.Linq;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Responses
{
    [Serializable]
    //[JsonObject(MemberSerialization.OptIn)]
    public sealed class JoinResponse
        : DurakResponseBase
    {
        // [JsonProperty(PropertyName = "players")]
        // private PlayerInfo[] _players;
        //
        // [JsonProperty(PropertyName = "ready_players")]
        // private PlayerInfo[] _readyPlayers;
        //
        // public IReadOnlyList<PlayerInfo> players => _players?
        //     .Where(value => value != null)
        //     .ToArray();
        //
        // public IReadOnlyList<PlayerInfo> ready_players => _readyPlayers?
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