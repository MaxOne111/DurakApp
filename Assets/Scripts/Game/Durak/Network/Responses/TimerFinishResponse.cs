using System;
using Game.Durak.Network.Responses;

namespace Game.Durak.Network.Responses
{
    
    [Serializable]
    public sealed class TimerFinishResponse : DurakResponseBase
    {
        public int timer_id; // not used
    }

}