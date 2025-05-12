using System;
using Zenject;

namespace Game.Durak.Core
{
    public sealed class CardPresenter
        : IInitializable, IDisposable
    {
        public CardPresenter(CardModel model, CardView view)
        {
            _model = model;
            _view = view;
        }

        private readonly CardModel _model;
        private readonly CardView _view;
        
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}