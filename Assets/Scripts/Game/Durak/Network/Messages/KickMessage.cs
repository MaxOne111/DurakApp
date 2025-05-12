using System;
using Game.Durak.Enums;

namespace Game.Durak.Network.Messages
{
    public sealed class KickMessage
        : DurakMessageBase
    {
        public KickMessage() 
            : base(ETurnMode.Kick)
        {
            throw new NotImplementedException();
        }
    }
}