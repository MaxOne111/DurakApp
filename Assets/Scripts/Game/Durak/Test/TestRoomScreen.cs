using System.Linq;
using System.Threading.Tasks;
using Game.Durak.Core;
using Game.Durak.Network.Messages;
using Game.Durak.Network.Responses;
using Game.UI;
using Game.UI.Abstractions;
using ModestTree;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Game.Durak.Test
{
    public sealed class TestRoomScreen
        : MonoBehaviour
    {
        [SerializeField] private TableSettings tableSettings;

        private IFactory<PlayerInfo, Vector2, PlayerModel> _playerFactory;
        private IWaitingMarker _marker;

        [Inject]
        private void Construct
        (
            IFactory<PlayerInfo, Vector2, PlayerModel> playerFactory, 
            IWaitingMarker marker
        )
        {
            _playerFactory = playerFactory;
            _marker = marker;
        }
        
        public async Task Play(DurakConfiguration configuration)
        {
            var waitingSession = await DurakHelper
                .CreateSession(configuration)
                .UseMarker(_marker);

            var players = new PlayerModel[configuration.PlayerCount];
            for (var i = 0; i < configuration.PlayerCount; i++)
            {
                var position = tableSettings.ChairPositions[configuration.PlayerCount][i];
                players[i] = _playerFactory.Create(null, position);
            }

            var joinResponse = await waitingSession.Join(new JoinMessage(new PlayerInfo("Test", 0,0)));
            players[0].SetInfo(joinResponse.players[0]);
            
            waitingSession.OnSomeoneJoin += HandleJoin;
            waitingSession.OnSomeoneDisconnect += HandleDisconnect;

            var preparingSession = await waitingSession.WaitForFullness();

            waitingSession.OnSomeoneJoin -= HandleJoin;

            preparingSession.OnSomeoneReady += HandleReady;
            var (gameStartedResponse, activeSession) = await preparingSession.WaitForStart();
            
            waitingSession.OnSomeoneDisconnect -= HandleDisconnect;
            
            while (true)
            {
                await Task.Yield();
            }

            void HandleJoin(JoinResponse response)
            {
                Debug.Log($"HandleJoin.response: {JsonConvert.SerializeObject(response)}");
                
                if (response.players == null)
                {
                    return;
                }
                
                for (var i = 0; i < response.players.Length; i++)
                {
                    players[i].SetInfo(response.players[i]);
                }
            }

            void HandleDisconnect(DisconnectResponse response)
            {
                Debug.Log($"HandleDisconnect.response: {JsonConvert.SerializeObject(response)}");
                
                if (response.Players == null)
                {
                    return;
                }
                
                for (var i = 0; i < response.Players.Count; i++)
                {
                    players[i].SetInfo(response.Players[i]);
                }
            }

            void HandleReady(ReadyResponse response)
            {
                Debug.Log($"HandleReady.response: {JsonConvert.SerializeObject(response)}");
                
                if (response.players == null || response.ready_players == null)
                {
                    return;
                }
                
                for (var i = 0; i < response.players.Length; i++)
                {
                    players[i].SetInfo(response.players[i]);
                }
                
                for (var i = 0; i < response.ready_players.Length; i++)
                {
                    var index = players
                        .Select(value => value.Info)
                        .ToArray()
                        .IndexOf(response.ready_players[i]);
                    
                    players[index].SetReady();
                }
            }
        }
    }
}