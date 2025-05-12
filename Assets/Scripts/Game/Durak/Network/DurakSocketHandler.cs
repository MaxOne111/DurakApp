using System;
using Infrastructure.Network;

namespace Game.Durak.Network
{
    public sealed class DurakSocketHandler
        : WebSocketPingPongHandlerBase
    {
        public DurakSocketHandler(int id)
            : base(TimeSpan.FromHours(1))
        {
            _id = id;
        }

        private const int Port = 8000;

        private readonly int _id;

        protected override Uri GetUri()
        {
            var link = @$"ws://188.225.84.38:{Port}/api/ws/{_id}";
            
            var canCreate = Uri.TryCreate(link, UriKind.Absolute, out var result);
            if (!canCreate)
            {
                throw new ArgumentException();
            }

            return result;
        }
    }
}