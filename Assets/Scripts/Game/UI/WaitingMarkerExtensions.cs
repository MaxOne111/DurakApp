using System.Threading.Tasks;
using Game.UI.Abstractions;

namespace Game.UI
{
    public static class WaitingMarkerExtensions
    {
        public static async Task UseMarker(this Task self, IWaitingMarker marker)
        {
            var token = marker.Lock();
            
            while (!self.IsCompleted)
            {
                await Task.Yield();
            }
            
            token.Dispose();
        }

        public static async Task<T> UseMarker<T>(this Task<T> self, IWaitingMarker marker)
        {
            var token = marker.Lock();
            
            while (!self.IsCompleted)
            {
                await Task.Yield();
            }
            
            token.Dispose();

            return self.Result;
        }
    }
}