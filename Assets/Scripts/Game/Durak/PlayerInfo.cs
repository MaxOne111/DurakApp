using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak
{
    [Serializable]
    public sealed class PlayerInfo
    {
        [JsonConstructor]
        public PlayerInfo([JsonProperty(PropertyName = "username")] string name, [JsonProperty(PropertyName = "user_id")] int id, [JsonProperty(PropertyName = "balance")] int balance)
        {
            username = name;
            user_id = id;
            this.balance = balance;
        }

        // [JsonProperty(PropertyName = "username")]
        // public string Name
        // {
        //     get;
        //     set;
        // }
        //
        // [JsonProperty(PropertyName = "user_id")]
        // public int Id
        // {
        //     get;
        //     set;
        // }

        public string username;
        public int user_id;
        public int balance;
        public EPlayerRole Turn;
        public CardInfo min_trump_card;

        public static implicit operator PlayerInfo(long value) => null;
    }
}