using System;
using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Responses
{
    [Serializable]
    public sealed class ErrorResponse : DurakResponseBase

    {
        // [JsonProperty(PropertyName = "Error")]
        // public ELogicError Error { get; set; }

        public ELogicError Error;
        public TournamentRoomMessage next_game;
    }
}