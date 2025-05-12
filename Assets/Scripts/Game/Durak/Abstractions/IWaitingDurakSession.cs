using System;
using System.Threading.Tasks;
using Game.Durak.Network.Messages;
using Game.Durak.Network.Responses;

namespace Game.Durak.Abstractions
{
    public interface IWaitingDurakSession
        : IDurakSession
    {
        public event Action<JoinResponse> OnSomeoneJoin;

        public Task<JoinResponse> Join(JoinMessage message);


        public Task<IPreparingDurakSession> WaitForFullness();
    }
}