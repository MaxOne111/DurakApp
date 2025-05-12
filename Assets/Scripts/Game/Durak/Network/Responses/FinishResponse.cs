using System;
using Game.Durak;

namespace Game.Durak.Network.Responses
{
    [Serializable]
    public sealed class FinishResponse : DurakResponseBase
    {
        public PlayerInfoFinish[] winners;
        public PlayerInfoFinish[] losers;
    }
    
}

[Serializable]
public struct PlayerInfoFinish
{
    public PlayerInfo user;
    public decimal amount;
    public int room_id;
}