using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DataContract]
    public enum EStatusMessage
        //: byte
    {
        [EnumMember(Value = "")]
        Waiting = EGameStatus.Waiting,
        
        [EnumMember(Value = "game in ready")]
        Ready = EGameStatus.Ready,
        
        [EnumMember(Value = "")]
        Game = EGameStatus.Game,
        
        [EnumMember(Value = "")]
        End = EGameStatus.End
    }
}