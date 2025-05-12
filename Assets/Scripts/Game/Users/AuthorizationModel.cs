using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Users
{
    public sealed class AuthorizationModel
    {
        public event Func<string, string, string> OnAuthorize = (_, _) => string.Empty;

        public string Authorize(string login, string password)
        {
            var result = OnAuthorize.Invoke(login, password);
            return result;
        }
    }
}
