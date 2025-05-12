using System;
using Game.Durak.Network.Responses;

namespace Game.Durak.Network.Responses
{
    [Serializable]
    public sealed class TimerDataResponse : DurakResponseBase
    {
        public Timer timer;
    }
}