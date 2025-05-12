using System;
using WebSocketSharp;

namespace Game.Durak.SelectLobby
{
    public static class SelectLobbyHelper
    {
        private const string Url = "ws://durak.sino0on.ru/api/games";
        
        private static readonly Lazy<SelectLobbyService> LazyService 
            = new Lazy<SelectLobbyService>(CreateService);

        public static SelectLobbyService Service => LazyService.Value;

        private static SelectLobbyService CreateService()
        {
            var socket = new WebSocket(Url);
            
            var result = new SelectLobbyService(socket);
            return result;
        }
    }
}