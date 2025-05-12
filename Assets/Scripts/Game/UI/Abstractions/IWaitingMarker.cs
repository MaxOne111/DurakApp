using System;

namespace Game.UI.Abstractions
{
    public interface IWaitingMarker
    {
        IDisposable Lock();
    }
}