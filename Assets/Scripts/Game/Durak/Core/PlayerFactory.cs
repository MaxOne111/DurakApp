using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.Durak.Core
{
    public sealed class PlayerFactory
        : IFactory<PlayerInfo, Vector2, PlayerModel>, IDisposable
    {
        public PlayerFactory(PlayerView prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        private readonly PlayerView _prefab;
        private readonly Transform _parent;

        private readonly ICollection<IDisposable> _disposables = new List<IDisposable>();

        public PlayerModel Create(PlayerInfo info, Vector2 position)
        {
            var result = new PlayerModel();
            
            var view = Object.Instantiate(_prefab, _parent);
            ((RectTransform) view.transform).anchoredPosition = position;
            
            var presenter = new PlayerPresenter(result, view);
            presenter.Initialize();
            
            result.SetInfo(info);
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