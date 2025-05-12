using System;
using Infrastructure;

namespace Game.Users
{
    public static class UserHelper
    {
        private static readonly Lazy<UserService> LazyService 
            = new Lazy<UserService>(CreateService);

        public static UserService Service => LazyService.Value;
        
        private static UserService CreateService()
        {
            var result = new UserService(WebHelper.Client);
            return result;
        }
    }
}