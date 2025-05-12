using System;
using Game.Durak;
using Game.Durak.Enums;

namespace Game.Durak.Network.Responses
{
    [Serializable]
    public sealed class InfoResponse : DurakResponseBase

    {
        // [JsonProperty(PropertyName = "Info")]
        // public ELogicInfo Info { get; set; }
        //
        // [JsonProperty(PropertyName = "mode")]
        // public EResponseMode Mode { get; set; }

        public ELogicInfo Info;
        public string value;
        public ETurnMode action;
        public PlayerInfo user;
    }
}

[Serializable]
public struct Timer
{
    public string time_zone;
    public string now;
    public string creation_time;
    public string expiration_time;
}

[Serializable]
public struct PlayerCashInfo
{
    public PlayerInfo user;
    public int cash;
}