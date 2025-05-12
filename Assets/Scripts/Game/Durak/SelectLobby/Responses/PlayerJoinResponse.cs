using System.Collections.Generic;
using Game.Durak.SelectLobby.Enums;
using Newtonsoft.Json;

/*
 * Example:
{
    "self.lobby_id": "1016",
    "players": [
        {
            "username": "None",
            "user_id": 0,
            "passed": false,
            "status": null
        },
        {
            "username": "None",
            "user_id": 0,
            "passed": false,
            "status": null
        }
    ],
    "max_players": 2,
    "action": "join"
}
*/

namespace Game.Durak.SelectLobby.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class PlayerJoinResponse
    {
        [JsonConstructor]
        public PlayerJoinResponse
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