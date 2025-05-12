using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
using WebSocketSharp;

namespace Infrastructure.Network
{
    public abstract class WebSocketPingPongHandlerBase
        : IPingPongHandler, IInitializable<Task>, IDisposable
    {
        protected WebSocketPingPongHandlerBase(TimeSpan keepAliveInterval)
        {
            _keepAliveInterval = keepAliveInterval;
        }

        private static readonly TimeSpan ListenDelay = TimeSpan.FromSeconds(1);
    
        private readonly TimeSpan _keepAliveInterval;
        
        private readonly ClientWebSocket _client = new ClientWebSocket();
        private readonly CancellationTokenSource _source = new CancellationTokenSource();
        
        protected abstract Uri GetUri();

        public async Task Listen(Action<string> handler, CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                var data = await Ping(string.Empty);
                if (!data.IsNullOrEmpty())
                {
                    handler.Invoke(data);
                }

                await Task.Delay(ListenDelay, token);
            }
        }

        public async Task<string> Ping(string message)
        {
            Debug.Log($"Ping: {message}");
            
            try
            {
                var pingBytes = Encoding.UTF8.GetBytes(message);
                
                await _client.SendAsync(pingBytes, WebSocketMessageType.Text, true, _source.Token);
                var pongBytes = new ArraySegment<byte>(new byte[1024]);
                
                var result = await _client.ReceiveAsync(pongBytes, _source.Token);

                var decoded = Encoding.UTF8.GetString(pongBytes.Array, 0, result.Count);
                Debug.Log($"Pong: {decoded}");

                return decoded;
            }
            catch
            {
                Debug.Log($"Pong: {string.Empty}");
                return string.Empty;
            }
        }
        
        public async Task Initialize()
        {
            var uri = GetUri();

            _client.Options.KeepAliveInterval = _keepAliveInterval;
            await _client.ConnectAsync(uri, _source.Token);
        }

        public void Dispose()
        {
            _client.Dispose();
            _source.Dispose();
        }
    }
}