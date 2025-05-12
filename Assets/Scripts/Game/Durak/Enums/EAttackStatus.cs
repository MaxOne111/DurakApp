using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DataContract]
    public enum EAttackStatus
    {
        [EnumMember(Value = "took")]
        Took,
        
        [EnumMember(Value = "in progress")]
        InProgress,
        
        [EnumMember(Value = "beat")]
        Beat,
        
        [EnumMember(Value = "pending_take")]
        Pending_Take,
        
        [EnumMember(Value = "in_game")]
        In_Game,
        
        [EnumMember(Value = "waiting")]
        Waiting,
        
        [EnumMember(Value = "ready")]
        Ready,
        
        [EnumMember(Value = "game in ready")]
        GameInReady,
        
        [EnumMember(Value = "take")]
        Take,
    }
}