using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Messages
{
    [Serializable]
    public sealed class DefenseMessage
        : DurakMessageBase
    {
        public DefenseMessage(PlayerInfo target, CardInfo card, int slot) 
            : base(ETurnMode.Defence)
        {
            user = target;
            this.card = card;
            this.slot = slot;
        }

        // [JsonProperty(PropertyName = "user")]
        // public PlayerInfo User
        // {
        //     get;
        // }
        //
        // [JsonProperty(PropertyName = "card")]
        // public CardInfo Card
        // {
        //     get;
        // }
        //
        // [JsonProperty(PropertyName = "slot")]
        // public int Slot
        // {
        //     get;
        // }

        public PlayerInfo user;
        public CardInfo card;
        public int slot;
    }
}