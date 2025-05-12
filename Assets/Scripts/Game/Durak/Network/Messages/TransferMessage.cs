using System;
using Game.Durak.Enums;

namespace Game.Durak.Network.Messages
{
    public sealed class TransferMessage
        : DurakMessageBase
    {
        public TransferMessage(PlayerInfo target, CardInfo card) 
            : base(ETurnMode.Transfer)
        {
            user = target;
            this.card = card;
        }
        
        public PlayerInfo user;
        public CardInfo card;
    }
}