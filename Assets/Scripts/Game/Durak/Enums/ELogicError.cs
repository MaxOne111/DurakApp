using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DataContract]
    public enum ELogicError
    {
        [EnumMember(Value = "")]
        Test,
    
        [EnumMember(Value = "You can't beat")]
        WeakCard,
        
        [EnumMember(Value = "You can't press beat")]
        EarlyBeat,
        
        [EnumMember(Value = "You can't press pass")]
        EarlyPass,
    
        [EnumMember(Value = "Not rank in table")]
        MissingTableCard,
    
        [EnumMember(Value = "Not card in your hands")]
        MissingPlayerCard,
    
        [EnumMember(Value = "You can't beat with open slots")]
        OpenSlot,
        
        [EnumMember(Value = "You are not Init")]
        NotInit,
        
        [EnumMember(Value = "You are not Enemy")]
        NotEnemy,
        
        [EnumMember(Value = "Wait Init")]
        WaitInit,
        
        [EnumMember(Value = "Slot closed")]
        SlotClosed,
        
        [EnumMember(Value = "Not your turn to attack")]
        NotYourTurn,
        
        [EnumMember(Value = "Maximum slots")]
        MaximumSlots,
        
        [EnumMember(Value = "You can't transfer")]
        CanNotTransfer,
        
        [EnumMember(Value = "User already connected to this lobby")]
        AlreadyInLobby,
        
        [EnumMember(Value = "Room not found")]
        RoomNotFound,
        
        [EnumMember(Value = "Room is already in game")]
        RoomInGame,
        
        [EnumMember(Value = "Incorrect json")]
        IncorrectJson,
        
        [EnumMember(Value = "Enemy not found")]
        EnemyNotFound,
        
        [EnumMember(Value = "Init not found")]
        InitNotFound,
        
        [EnumMember(Value = "Player not found")]
        PlayerNotFound,
        
        [EnumMember(Value = "you can't beat")]
        CantBeat,
        
        [EnumMember(Value = "Not your turn to take cards")]
        NotYoutTurnToTakeCards,
        
        [EnumMember(Value = "Timer not found")]
        TimerNotFound,
        
        [EnumMember(Value = "Defender cannot pass")]
        DefenderCantPass,
        
        [EnumMember(Value = "No empty slots available in the room.")]
        NoEmptySlots,
        
        [EnumMember(Value = "Tournament room not found")]
        TournamentRoomNotFound,

    }
    
    
}