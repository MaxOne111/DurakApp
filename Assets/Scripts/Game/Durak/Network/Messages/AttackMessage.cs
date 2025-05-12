using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Messages
{
    [Serializable]
    public sealed class AttackMessage
        : DurakMessageBase
    {
        public AttackMessage(PlayerInfo target, CardInfo card)
            : base(ETurnMode.Attack)
        {
            user = target;
            this.card = card;
        }

        // [JsonProperty(PropertyName = "user")]
        // public PlayerInfo Target
        // {
        //     get;
        // }
        //
        // [JsonProperty(PropertyName = "card")]
        // public CardInfo Card
        // {
        //     get;
        // }

        public PlayerInfo user;
        public CardInfo card;
    }
}