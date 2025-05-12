using System;
using Game.Durak.Enums;

namespace Game.Durak.Network.Responses
{
    [Serializable]
    public sealed class TakeResponse
        : DurakResponseBase
    {
        public string username; //not used
        public int user_id; //not used
        public bool passed; //not used
        public CardInfo[] cards;
        public PlayerCardInfo[] players;
        public CardInfo[] get_cards; //not used
        public PlayerInfo taker;
    }
}