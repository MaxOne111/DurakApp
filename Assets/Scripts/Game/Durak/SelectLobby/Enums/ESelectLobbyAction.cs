using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.SelectLobby.Enums
{
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ESelectLobbyAction
    {
        [EnumMember(Value = "join")]
        Join,
        
        [EnumMember(Value = "kick")]
        Kick
    }
}