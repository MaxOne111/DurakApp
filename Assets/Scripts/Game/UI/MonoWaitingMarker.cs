using System;
using Game.UI.Abstractions;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.UI
{
    public sealed class MonoWaitingMarker
        : MonoBehaviour, IWaitingMarker
    {
        [SerializeField] private GameObject root;
        
        [CanBeNull] 
        private IWaitingMarker _inner;

        [CanBeNull] 
        private IDisposable _innerToken;
        
        public IDisposable Lock()
        {
            if (_inner != null)
            {
                _innerToken?.Dispose();
            }

            _inner = new WaitingMarker(SetLocked);
            _innerToken = _inner.Lock();

            return _innerToken;
        }

        private void SetLocked(bool isLocked)
        {
            root.SetActive(isLocked);

            if (isLocked) return;
            
            _inner = null;
            _innerToken = null;
        }
    }
}