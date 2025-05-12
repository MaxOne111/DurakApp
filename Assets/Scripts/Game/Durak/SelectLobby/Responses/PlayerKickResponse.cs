using System.Collections.Generic;
using Game.Durak.SelectLobby.Enums;
using Newtonsoft.Json;

namespace Game.Durak.SelectLobby.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class PlayerKickResponse
    {
        [JsonConstructor]
        public PlayerKickResponse
        (
            [JsonProperty(PropertyName = "self.lobby_id")] string lobbyId,
            [JsonProperty(PropertyName = "players")] PlayerInfo[] players,
            [JsonProperty(PropertyName = "max_players")] int maxPlayers,
            [JsonProperty(PropertyName = "action")] ESelectLobbyAction action
        )
        {
            LobbyId = lobbyId;
            Players = players;
            MaxPlayers = maxPlayers;
            Action = action;
        }
        
        public string LobbyId
        {
            get;
        }

        public IReadOnlyList<PlayerInfo> Players
        {
            get;
        }

        public int MaxPlayers
        {
            get;
        }

        public ESelectLobbyAction Action
        {
            get;
        }
    }
}