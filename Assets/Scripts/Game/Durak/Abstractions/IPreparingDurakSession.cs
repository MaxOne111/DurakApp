using System;
using System.Threading.Tasks;
using Game.Durak.Network.Messages;
using Game.Durak.Network.Responses;

namespace Game.Durak.Abstractions
{
    public interface IPreparingDurakSession
        : IDurakSession
    {
        public event Action<ReadyResponse> OnSomeoneReady;        
        
        public Task<ReadyResponse> Ready(ReadyMessage message);

        public Task<(GameStartedResponse, IActiveDurakSession)> WaitForStart();
    }
}