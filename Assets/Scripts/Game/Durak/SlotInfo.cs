using System;
using Newtonsoft.Json;

namespace Game.Durak
{
    [Serializable]
    public sealed class SlotInfo
    {
        // [JsonProperty(PropertyName = "init")]
        // public int Init
        // {
        //     get;
        //     set;
        // }
        //
        // [JsonProperty(PropertyName = "init_card")]
        // public CardInfo InitCard
        // {
        //     get;
        //     set;
        // }
        //
        // [JsonProperty(PropertyName = "status")]
        // public bool Status
        // {
        //     get;
        //     set;
        // }
        //
        // [JsonProperty(PropertyName = "enemy")]
        // public int Enemy
        // {
        //     get;
        //     set;
        // }
        //
        // [JsonProperty(PropertyName = "enemy_card")]
        // public CardInfo EnemyCard
        // {
        //     get;
        //     set;
        // }
        
        public int index;
        
        public int init;
        public CardInfo init_card;
        public bool status;
        public int enemy;
        public CardInfo enemy_card;
    }
}