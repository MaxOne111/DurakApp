using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DataContract]
    public enum EBalanceType
    {
        [EnumMember(Value = "BALANCE")]
        Default,
        
        [EnumMember(Value = "BONUS_BALANCE")]
        Bonus,
        
        [EnumMember(Value = "CLUB_BALANCE")]
        Club
    }
}