using System;
using Zenject;

namespace Game.Durak.Core
{
    public sealed class TablePresenter
        : IInitializable, IDisposable
    {
        public TablePresenter(TableModel model)
        {
            _model = model;
        }

        private readonly TableModel _model;
        
        public void Initialize()
        {
            _model.OnChairsCountChanged += HandleChairsCountChanged;
        }

        public void Dispose()
        {
            _model.OnChairsCountChanged -= HandleChairsCountChanged;
        }

        private void HandleChairsCountChanged(int count)
        {
            
        }
    }
}