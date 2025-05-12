using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak
{
    [Serializable]
    public sealed class CardInfo
    {
        public CardInfo(ECardSuit suit, ECardRank rank)
        {
            this.suit = suit;
            this.rank = rank;
        }
        
        // [JsonProperty(PropertyName = "suit")]
        // public ECardSuit Suit
        // {
        //     get;
        // }
        //
        // [JsonProperty(PropertyName = "rank")]
        // public ECardRank Rank
        // {
        //     get;
        // }

        public ECardSuit suit;
        public ECardRank rank;
    }
}