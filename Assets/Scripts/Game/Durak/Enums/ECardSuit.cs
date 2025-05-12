using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DataContract]
    public enum ECardSuit
    {
        [EnumMember(Value = "spades")]
        Spades,
        
        [EnumMember(Value = "clubs")]
        Clubs,
        
        [EnumMember(Value = "hearts")]
        Hearts,
        
        [EnumMember(Value = "diamonds")]
        Diamonds
    }
}