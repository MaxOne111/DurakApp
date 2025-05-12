using System;
using UnityEngine.Events;

namespace Utils
{
    public static class UnityEventExtensions
    {
        public static IDisposable AddDisposableListener(this UnityEvent self, UnityAction action)
        {
            self.AddListener(action);
            
            var result = new CallbackDisposable(() => self.RemoveListener(action));
            return result;
        }
        
        public static IDisposable AddDisposableListener<T>(this UnityEvent<T> self, UnityAction<T> action)
        {
            self.AddListener(action);
            
            var result = new CallbackDisposable(() => self.RemoveListener(action));
            return result;
        }
    }
}