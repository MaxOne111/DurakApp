using Game.Durak.Enums;

namespace Game.Durak.Network.Messages
{
    public sealed class WatchMessage
        : DurakMessageBase
    {
        public WatchMessage(PlayerInfo info) 
            : base(ETurnMode.Watch)
        {
            user = info;
        }
        
        public PlayerInfo user;
    }
}