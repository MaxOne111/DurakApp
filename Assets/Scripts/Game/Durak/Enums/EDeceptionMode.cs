using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DataContract]
    public enum EDeceptionMode
    {
        [EnumMember(Value = "deception")]
        Deception,
    }
}