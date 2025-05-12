using JetBrains.Annotations;
using UnityEngine;

namespace Game.Bootstrapping
{
#if UNITY_ANDROID
    public static class AndroidBootstrapMethods
    {
        [UsedImplicitly]
        [BootstrapMethod]
        public static void SetTargetFramerate()
        {
            var targetFramerate = (int) Screen.currentResolution.refreshRateRatio.value;
            Application.targetFrameRate = targetFramerate;

            Debug.Log($"Just set target framerate to {targetFramerate}");
        }
    }
#endif
}