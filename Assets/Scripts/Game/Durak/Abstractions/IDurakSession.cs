using System;
using System.Threading.Tasks;
using Game.Durak.Enums;
using Game.Durak.Network.Messages;
using Game.Durak.Network.Responses;

namespace Game.Durak.Abstractions
{
    public interface IDurakSession
    {
        public event Action<DisconnectResponse> OnSomeoneDisconnect;

        EGameStatus Status
        {
            get;
        }
    }
}