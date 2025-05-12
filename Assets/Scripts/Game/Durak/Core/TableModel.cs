using System;

namespace Game.Durak.Core
{
    public sealed class TableModel
    {
        public event Action<int> OnChairsCountChanged = _ => { };

        public int ChairsCount
        {
            get;
            private set;
        }
        
        public void SetChairsCount(int count)
        {
            ChairsCount = count;
            OnChairsCountChanged.Invoke(ChairsCount);
        }
    }
}