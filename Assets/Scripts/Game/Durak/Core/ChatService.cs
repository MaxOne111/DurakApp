using System;
using Game.Durak.Enums;
using Game.Durak.Network.Messages;
using Newtonsoft.Json;
using WebSocketSharp;
using Zenject;

namespace Game.Durak.Core
{
    public sealed class ChatService
        : IInitializable, IDisposable
    {
        public ChatService(WebSocket socket)
        {
            _socket = socket;
        }
        
        public event Action<int, EChatMessageType, string> OnMessage = (_, _, _) => { };

        private readonly WebSocket _socket;
        
        public void Initialize()
        {
            _socket.OnMessage += HandleMessage;
        }

        public void Dispose()
        {
            _socket.OnMessage -= HandleMessage;
        }

        private void HandleMessage(object sender, MessageEventArgs args)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<ChatMessage>(args.Data);
                
            }
            catch 
            {
                // ignore
            }
        }
    }
}