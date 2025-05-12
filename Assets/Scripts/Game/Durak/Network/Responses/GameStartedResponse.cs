using System;
using System.Collections.Generic;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Responses
{
    //[JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public sealed class GameStartedResponse : DurakResponseBase

    {
    // [JsonProperty(PropertyName = "cards", Order = 2)] 
    // private CardInfo[] _cards;
    //
    // [JsonProperty(PropertyName = "username")]
    // public string Username
    // {
    //     get;
    //     set;
    // }
    //
    // [JsonProperty(PropertyName = "user_id")]
    // public int UserId
    // {
    //     get;
    //     set;
    // }
    //
    // public IReadOnlyList<CardInfo> Cards => _cards;
    //
    // [JsonProperty(PropertyName = "trump")]
    // public CardInfo Trump
    // {
    //     get;
    //     set;
    // }

    public CardInfo[] cards;
    public CardInfo trump;
    public PlayerCardInfo[] players;
    }
}