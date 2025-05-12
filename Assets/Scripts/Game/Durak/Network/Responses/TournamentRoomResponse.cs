using System;

namespace Game.Durak.Network.Responses
{
    [Serializable]
    public sealed class TournamentRoomResponse : DurakResponseBase
    {
        public TournamentRoomMessage next_game;
    }
}

