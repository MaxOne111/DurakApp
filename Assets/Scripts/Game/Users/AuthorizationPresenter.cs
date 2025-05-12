using System;
using Zenject;

namespace Game.Users
{
    public sealed class AuthorizationPresenter
        : IInitializable, IDisposable
    {
        public AuthorizationPresenter(AuthorizationModel model, AuthorizationView view)
        {
            
        }
        
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