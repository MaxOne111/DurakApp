using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DataContract]
    public enum ETurnMode
    {
        [EnumMember(Value = "join")]
        Join,
        
        [EnumMember(Value = "ready")]
        Ready,
        
        [EnumMember(Value = "role")]
        Role,
        
        [EnumMember(Value = "attack")]
        Attack,
        
        [EnumMember(Value = "defence")]
        Defence,
        
        [EnumMember(Value = "beat")]
        Beat,
        
        [EnumMember(Value = "pass")]
        Pass,
        
        [EnumMember(Value = "error")]
        Error,
        
        [EnumMember(Value = "kick")]
        Kick,
        
        [EnumMember(Value = "take")]
        Take,
        
        [EnumMember(Value = "transfer")]
        Transfer,
        
        [EnumMember(Value = "reconnect")]
        Reconnect,
        
        [EnumMember(Value = "watch")]
        Watch,
        
        [EnumMember(Value = "swap")]
        Swap,

        [EnumMember(Value = "text")]
        Text,
        
        [EnumMember(Value = "get_timer")]
        GetTimer,
        
        [EnumMember(Value = "expired")]
        Expired,
        
        [EnumMember(Value = "cancelled")]
        Cancelled,
        
        [EnumMember(Value = "timer_game")]
        TimerGame,
        
        [EnumMember(Value = "timer_ready")]
        TimerReady,
        
        [EnumMember(Value = "start_distribution")]
        StartDistribution,
        
        [EnumMember(Value = "info")]
        Info,
        
        [EnumMember(Value = "status")]
        Status,
        
        [EnumMember(Value = "finish")]
        Finish,
        
        [EnumMember(Value = "tournament")]
        Tournament,
        
        Unknown,
        
        // [EnumMember(Value = "info")]
        // Info,
        // [EnumMember(Value = "status")]
        // Status,
        // [EnumMember(Value = "status")]
        // Status,
        
    }
}

[JsonConverter(typeof(StringEnumConverter))]
[DataContract]
public enum ETurn
{
    [EnumMember(Value = "attack")]
    Attack,
    
    [EnumMember(Value = "defence")]
    Defence,
    
    [EnumMember(Value = "wait")]
    Wait,
}