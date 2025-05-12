using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DataContract]
    public enum EWhoTransferMode
    {
        [EnumMember(Value = "neighbours")]
        Neighbours,
        
        [EnumMember(Value = "all")]
        All
    }
}