using System;
using System.Collections.Generic;
using Game.Durak.SelectLobby.Enums;
using Game.Durak.SelectLobby.Responses;
using UnityEngine;
using Utils;
using WebSocketSharp;
using Zenject;

namespace Game.Durak.SelectLobby
{
    public sealed class SelectLobbyService
        : IInitializable, IDisposable
    {
        public SelectLobbyService(WebSocket socket)
        {
            _socket = socket;
        }

        public event Action<IEnumerable<LobbyInfo>> OnGetLobbies = _ => { };
        public event Action<PlayerJoinResponse> OnPlayerJoin = _ => { };
        public event Action<PlayerKickResponse> OnPlayerKick = _ => { };

        private readonly WebSocket _socket;
        
        public void Initialize()
        {
            try
            {
                _socket.OnOpen += HandleOpen;
                _socket.OnMessage += HandleMessage;
                _socket.OnError += HandleError;
                _socket.OnClose += HandleClose;

                _socket.Connect();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Dispose()
        {
            try
            {
                _socket.Close();

                _socket.OnOpen -= HandleOpen;
                _socket.OnMessage -= HandleMessage;
                _socket.OnError -= HandleError;
                _socket.OnClose -= HandleClose;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void HandleOpen(object sender, EventArgs args)
        {
            Debug.Log($"SelectLobbyService.HandleOpen");
        }

        private void HandleMessage(object sender, MessageEventArgs args)
        {
            try
            {
                Debug.Log(args.Data);

                var isGetLobbies = args.Data.TryDeserialize<LobbyInfo[]>(out var lobbies);
                if (isGetLobbies)
                {
                    OnGetLobbies.Invoke(lobbies);
                    return;
                }

                var isJoin = args.Data.TryDeserialize<PlayerJoinResponse>(out var joinResponse)
                             && joinResponse.Action == ESelectLobbyAction.Join;

                if (isJoin)
                {
                    OnPlayerJoin.Invoke(joinResponse);
                    return;
                }

                var isKick = args.Data.TryDeserialize<PlayerKickResponse>(out var kickResponse)
                             && kickResponse.Action == ESelectLobbyAction.Kick;

                if (isKick)
                {
                    OnPlayerKick.Invoke(kickResponse);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void HandleError(object sender, ErrorEventArgs args)
        {
            Debug.Log($"SelectLobbyService.HandleError({args.Message})");
            Debug.LogException(args.Exception);
        }

        private void HandleClose(object sender, CloseEventArgs args)
        {
            Debug.Log($"SelectLobbyService.HandleClose({args.Reason}, {args.Code}, {args.WasClean})");
        }
    }
}