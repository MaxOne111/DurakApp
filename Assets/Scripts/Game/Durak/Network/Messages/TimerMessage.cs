using System;
using Game.Durak.Enums;

namespace Game.Durak.Network.Messages
{
    [Serializable]
    public sealed class TimerMessage
        : DurakMessageBase
    {
        public TimerMessage(PlayerInfo user ,int id) 
            : base(ETurnMode.GetTimer)
        {
            this.user = user;
            this.id = id;
        }

        public PlayerInfo user;
        public int id;
    }
}