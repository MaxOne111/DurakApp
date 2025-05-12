using System;
using Game.Durak.Enums;

namespace Game.Durak
{
    [Serializable]
    public sealed class RoundInfo
    {
        public PlayerInfo init;
        public PlayerInfo enemy;
        public EAttackStatus status;
        public SlotInfo[] slots;
        public bool beat;
    }
}