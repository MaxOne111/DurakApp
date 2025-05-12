using Game.Durak.Enums;
using Game.UI;

namespace Game.Durak.UI
{
    public sealed class GameSpeedToggleGroup
        : ToggleGroupBase<EGameSpeed>
    {
        
    }

    public enum EGameSpeed
    {
        Default,
        Fast
    }
}