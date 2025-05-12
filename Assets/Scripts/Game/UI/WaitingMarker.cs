using System;
using Game.UI.Abstractions;
using Utils;

namespace Game.UI
{
    public sealed class WaitingMarker
        : IWaitingMarker
    {
        public WaitingMarker(Action<bool> setLocked)
        {
            _setLocked = setLocked;
        }

        private readonly Action<bool> _setLocked;

        public IDisposable Lock()
        {
            _setLocked.Invoke(true);
            
            var result = new CallbackDisposable(Unlock);
            return result;
        }

        private void Unlock()
        {
            _setLocked.Invoke(false);
        }
    }
}