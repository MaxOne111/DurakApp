using System.Threading.Tasks;
using Game.Durak.Network.Messages;
using Game.Durak.Network.Responses;

namespace Game.Durak.Abstractions
{
    public interface IActiveDurakSession
        : IDurakSession
    {
        public Task<AttackResponse> Attack(AttackMessage message);
        public Task<DefenseResponse> Defense(DefenseMessage message);
        public Task<BeatResponse> Beat(BeatMessage message);
        public Task<TakeResponse> Take(TakeMessage message);
        public Task<TransferResponse> Transfer(TransferMessage message);
    }
}