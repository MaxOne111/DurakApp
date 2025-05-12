using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DataContract]
    public enum EPlayerRole
    {
        [EnumMember(Value = "")]
        Null,
        
        [EnumMember(Value = "You are init")]
        Attacker,
        
        [EnumMember(Value = "You are enemy")]
        Enemy,
        
        [EnumMember(Value = "Not your turn")]
        Waiting
    }
}