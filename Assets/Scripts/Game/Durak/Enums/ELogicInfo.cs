using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DataContract]
    public enum ELogicInfo
    {
        [EnumMember(Value = "")]
        Test,
    
        [EnumMember(Value = "You can now pass or throw more cards")]
        CanPass, //for neighbours
        
        [EnumMember(Value = "Defender is about to take the cards, you can pass or throw more")]
        DefenderTake,
        
        [EnumMember(Value = "Defender took the cards")]
        DefenderTook,
        
        [EnumMember(Value = "Deck size")]
        DeckSize,
        
        [EnumMember(Value = "Attacker skipped")]
        AttackerSkipped,
        
        [EnumMember(Value = "Init skipped")]
        InitSkipped,
        
        [EnumMember(Value = "User has logged out")]
        UserLogOut,
        
        [EnumMember(Value = "User is reconnecting")]
        UserReconnect,

        [EnumMember(Value = "New round")]
        NewRound,
        
        [EnumMember(Value = "Player is the last player and loses the game.")]
        PlayerLose, 
        
        [EnumMember(Value = "Player has won the game!")] 
        PlayerWon,
        
        [EnumMember(Value = "Your turn")]
        YourTurn,
        
        [EnumMember(Value = "Active player")]
        ActivePlayer,
        
        [EnumMember(Value = "Min Trump Card")]
        MinTrumpCard,
        
        [EnumMember(Value = "Active ready")]
        ActiveReady,
        
        [EnumMember(Value = "User is connecting")]
        Connecting,
        
        [EnumMember(Value = "User already connected to this room.")]
        UserInRoom,
        
    }
}
