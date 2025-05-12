using System;
using System.Collections.Generic;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.Durak.Core
{
    public sealed class CardFactory
        : IFactory<CardInfo, CardModel>, IDisposable
    {
        public CardFactory(CardView prefab)
        {
            _prefab = prefab;
        }

        private readonly CardView _prefab;

        private readonly ICollection<IDisposable> _disposables 
            = new List<IDisposable>();

        public CardModel Create(CardInfo info)
        {
            var result = new CardModel();
            var view = Object.Instantiate(_prefab);
            
            var presenter = new CardPresenter(result, view);
            presenter.Initialize();
            
            _disposables.Add(presenter);
            
            return result;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            
            _disposables.Clear();
        }
    }
}