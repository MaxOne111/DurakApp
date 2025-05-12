using System;

namespace Game.Durak.Core
{
    public sealed class PlayerModel
    {
        public event Action<PlayerInfo> OnSetInfo = _ => { };
        public event Action OnReady = () => { };

        public PlayerInfo Info
        {
            get;
            private set;
        }

        public bool IsReady
        {
            get;
            private set;
        }
        
        public void SetInfo(PlayerInfo info)
        {
            Info = info;
            OnSetInfo.Invoke(Info);
        }

        public void SetReady()
        {
            IsReady = true;
            OnReady.Invoke();
        }
    }
}