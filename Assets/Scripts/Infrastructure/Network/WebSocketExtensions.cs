using System;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

namespace Infrastructure.Network
{
    public static class WebSocketExtensions
    {
        public static async Task<bool> SendTextAsync(this WebSocket self, string data)
        {
            var source = new TaskCompletionSource<bool>();
            self.SendAsync(data, Callback);

            return await source.Task;

            void Callback(bool isCompleted)
            {
                Debug.Log($"Sent message: {data}. Result: {isCompleted}");
                source.SetResult(isCompleted);
            }
        }

        public static async Task<string> ReceiveTextAsync(this WebSocket self)
        {
            var source = new TaskCompletionSource<string>();
            
            self.OnMessage += HandleMessage;

            return await source.Task;
            
            void HandleMessage(object sender, MessageEventArgs args)
            {
                // if (args.Type != Opcode.Text)
                // {
                //     return;
                // }
                
                self.OnMessage -= HandleMessage;
                
                source.SetResult(args.Data);
            }
        }

        public static Task<string> PingText(this WebSocket self, string data)
        {
            self.Send(data);
            
            var result = self.ReceiveTextAsync();
            return result;
        }

        public static async Task<string> PingTextAsync(this WebSocket self, string data)
        {
            var isSuccess = await self.SendTextAsync(data);
            if (isSuccess)
            {
                var result = await self.ReceiveTextAsync();
                return result;
            }

            throw new ArgumentException();
        }
    }
}