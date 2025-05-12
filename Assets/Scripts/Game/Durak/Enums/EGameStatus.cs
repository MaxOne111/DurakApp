using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DataContract]
    public enum EGameStatus
        : byte
    {
        [EnumMember(Value = "game in waiting")]
        Waiting, // Собираем лобби
        
        [EnumMember(Value = "ready")]
        Ready, // Ждём готовности
        
        [EnumMember(Value = "game start")]
        Game, // Играем
        
        [EnumMember(Value = "end")]
        End, // Убираемся
        
        [EnumMember(Value = "in_game")]
        In_Game,
        
        [EnumMember(Value = "in progress")]
        InProgress,
        
        [EnumMember(Value = "game in ready")]
        GameInReady,
        
        [EnumMember(Value = "pending_take")] 
        Pending_Take,
    }
}