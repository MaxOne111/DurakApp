using System.Collections.Generic;
using System.Linq;
using Game.Durak.SelectLobby.Responses;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Durak.SelectLobby
{
    public sealed class SelectLobbyDebugger
        : MonoBehaviour
    {
        private void HandleGetLobbies(IEnumerable<LobbyInfo> lobbies)
        {
            Debug.Log($"Got some lobbies: {string.Join(", ", lobbies.Select(JsonConvert.SerializeObject))}");
        }

        private void HandlePlayerJoin(PlayerJoinResponse response)
        {
            Debug.Log($"Player is join lobby {response.LobbyId}");
        }

        private void HandlePlayerKick(PlayerKickResponse response)
        {
            Debug.Log($"Player is kicked from lobby {response.LobbyId}");
        }
        
        private void OnEnable()
        {
            SelectLobbyHelper.Service.OnGetLobbies += HandleGetLobbies;
            SelectLobbyHelper.Service.OnPlayerJoin += HandlePlayerJoin;
            SelectLobbyHelper.Service.OnPlayerKick += HandlePlayerKick;
            
            SelectLobbyHelper.Service.Initialize();
        }

        private void OnDisable()
        {
            SelectLobbyHelper.Service.OnGetLobbies -= HandleGetLobbies;
            SelectLobbyHelper.Service.OnPlayerJoin -= HandlePlayerJoin;
            SelectLobbyHelper.Service.OnPlayerKick -= HandlePlayerKick;
            
            SelectLobbyHelper.Service.Dispose();
        }
    }
}