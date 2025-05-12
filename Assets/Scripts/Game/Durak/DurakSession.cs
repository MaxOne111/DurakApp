using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Game.Durak.Abstractions;
using Game.Durak.Enums;
using Game.Durak.Network.Responses;
using Game.Durak.Network.Messages;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Network;
using Newtonsoft.Json;
using UnityEngine;
using Utils;
using WebSocketSharp;
using Zenject;

namespace Game.Durak
{
    public sealed class DurakSession
        : IInitializable, IDisposable, IWaitingDurakSession, IPreparingDurakSession, IActiveDurakSession
    {
        public DurakSession(WebSocket socket, EVerboseMode verboseMode = EVerboseMode.Warning)
        {
            _socket = socket;
            _verboseMode = verboseMode;
        }

        public event Action<GameStartedResponse> OnGameStarted = _ => { };
        public event Action<ReadyResponse> OnSomeoneReady = _ => { }; 
        public event Action<JoinResponse> OnSomeoneJoin = _ => { };
        public event Action<DisconnectResponse> OnSomeoneDisconnect = _ => { };
        public event Action<EGameStatus> OnStatusChanged = _ => { };

        private static readonly Lazy<IDictionary<EGameStatus, IEnumerable<ETurnMode>>> LazyAllowedTurns =
            new Lazy<IDictionary<EGameStatus, IEnumerable<ETurnMode>>>(CreateAllowedTurns);

        private readonly WebSocket _socket;
        private readonly EVerboseMode _verboseMode;

        private readonly CancellationTokenSource _source = new CancellationTokenSource();

        public EGameStatus Status
        {
            get;
            private set;
        }

        private static IDictionary<EGameStatus, IEnumerable<ETurnMode>> CreateAllowedTurns()
        {
            var result = new Dictionary<EGameStatus, IEnumerable<ETurnMode>>()
            {
                [EGameStatus.Waiting] = new ETurnMode[]
                {
                    ETurnMode.Join,
                    ETurnMode.Kick,
                    ETurnMode.Swap
                },
                
                [EGameStatus.Ready] = new ETurnMode[]
                {
                    ETurnMode.Ready
                },
                
                [EGameStatus.Game] = new ETurnMode[]
                {
                    ETurnMode.Attack,
                    ETurnMode.Defence,
                    ETurnMode.Beat,
                    ETurnMode.Transfer,
                    ETurnMode.Take
                },
            };

            return result;
        }

        public void Initialize()
        {
            _socket.OnMessage += CheckMessage;
            _socket.OnError += HandleError;
            _socket.OnClose += HandleClose;
            
            _socket.Connect();
        }

        public void Dispose()
        {
            _socket.OnMessage -= CheckMessage;
            _socket.OnError -= HandleError;
            _socket.OnClose -= HandleClose;
            
            _socket.Close();
            _source.Cancel();
        }

        public async Task<IPreparingDurakSession> WaitForFullness()
        {
            if (Status != EGameStatus.Waiting)
            {
                var log = $"Unable to start waiting for fullness when session status is {Status}!";
                VerboseProvider.Log(log, _verboseMode);
            }

            var isFull = false;

            OnSomeoneJoin += HandleJoin;
            await AsyncHelper.WaitUntil(() => isFull);
            OnSomeoneJoin -= HandleJoin;

            return this;

            void HandleJoin(JoinResponse response)
            {
                isFull = response.ready_players.Length == response.players.Length;
            }
        }
        
        public async Task<(GameStartedResponse, IActiveDurakSession)> WaitForStart()
        {
            if (Status != EGameStatus.Ready)
            {
                var log = $"Unable to start waiting for start when session status is {Status}!";
                VerboseProvider.Log(log, _verboseMode);
            }

            var source = new TaskCompletionSource<GameStartedResponse>();
            
            OnGameStarted += HandleGameStarted;
            var result = await source.Task;
            OnGameStarted -= HandleGameStarted;
            
            return (result, this);

            void HandleGameStarted(GameStartedResponse response)
            {
                source.SetResult(response);
            }
        }

        public Task<JoinResponse> Join(JoinMessage message) => SendTurnMessage<JoinMessage, JoinResponse>(message);
        public Task<ReadyResponse> Ready(ReadyMessage message) => SendTurnMessage<ReadyMessage, ReadyResponse>(message);

        public Task<AttackResponse> Attack(AttackMessage message) => SendTurnMessage<AttackMessage, AttackResponse>(message);
        public Task<DefenseResponse> Defense(DefenseMessage message) => SendTurnMessage<DefenseMessage, DefenseResponse>(message);
        public Task<BeatResponse> Beat(BeatMessage message) => SendTurnMessage<BeatMessage, BeatResponse>(message);
        public Task<TakeResponse> Take(TakeMessage message) => SendTurnMessage<TakeMessage, TakeResponse>(message);
        public Task<TransferResponse> Transfer(TransferMessage message) => SendTurnMessage<TransferMessage, TransferResponse>(message);

        private async Task<TResponse> SendTurnMessage<TMessage, TResponse>(TMessage message)
            where TMessage : DurakMessageBase
            where TResponse : DurakResponseBase
        {
            if (!IsTurnAllowed(message.Mode))
            {
                var log = $"Turn {message.Mode} is not allowed when session is in {Status} status!";
                VerboseProvider.Log(log, _verboseMode);
            }
            
            var joinMessage = JsonConvert.SerializeObject(message);
            var data = await _socket.PingText(joinMessage);
            var response = JsonConvert.DeserializeObject<TResponse>(data);
            Status = response.status;

            return response;
        }
        
        private bool IsTurnAllowed(ETurnMode turn)
        {
            var result = LazyAllowedTurns
                .Value[Status]
                .Contains(turn);
            
            return result;
        }
        
        private void CheckResponse<TResponse>(string data, ref Action<TResponse> callback, params Predicate<TResponse>[] predicates)
        {
            var result = JsonConvert.DeserializeObject<TResponse>(data);
            if (predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    if (!predicate.Invoke(result))
                    {
                        return;
                    }
                }
            }

            if (result != null)
            {
                callback.Invoke(result);
            }
        }

        private void CheckMessage(object sender, MessageEventArgs args)
        {   
            // if (args.Type != Opcode.Text)
            // {
            //     return;
            // }

            var data = args.Data;
            Debug.Log($"Received message: {data}");
                
            CheckResponse(data, ref OnGameStarted);
            CheckResponse(data, ref OnSomeoneJoin, value => value.players != null);
            CheckResponse(data, ref OnSomeoneDisconnect, value => value.Info == "user has logged out");
            CheckResponse(data, ref OnSomeoneReady, value => value.ready_players != null);
        }

        private void HandleError(object sender, ErrorEventArgs args)
        {
            Debug.Log($"Socket caught error: {args.Message} {args.Exception}");
            Debug.LogException(args.Exception);
        }

        private void HandleClose(object sender, CloseEventArgs args)
        {
            Debug.Log($"Socket was closed with reason: {args.Reason}");
        }
    }
}