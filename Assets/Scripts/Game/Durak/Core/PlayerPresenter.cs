using System;
using Zenject;

namespace Game.Durak.Core
{
    public sealed class PlayerPresenter
        : IInitializable, IDisposable
    {
        public PlayerPresenter(PlayerModel model, PlayerView view)
        {
            _model = model;
            _view = view;
        }

        private readonly PlayerModel _model;
        private readonly PlayerView _view;
        
        public void Initialize()
        {
            _view.SetReady(false);
            
            _model.OnSetInfo += HandleSetInfo;
            _model.OnReady += HandleReady;
        }

        public void Dispose()
        {
            _model.OnSetInfo -= HandleSetInfo;
            _model.OnReady -= HandleReady;
        }

        private void HandleSetInfo(PlayerInfo info)
        {
            if (info == null)
            {
                _view.SetNickname(string.Empty);
                return;
            }
            
            // TODO: AVATAR SELECTION
            _view.SetNickname(info.username);
        }

        private void HandleReady()
        {
            _view.SetReady(true);
        }
    }
}