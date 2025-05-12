using System;
using Game.Durak.Enums;

namespace Game.Durak.Network.Messages
{
    public sealed class SwapMessage
        : DurakMessageBase
    {
        public SwapMessage() 
            : base(ETurnMode.Swap)
        {
            throw new NotImplementedException();
        }
    }
}