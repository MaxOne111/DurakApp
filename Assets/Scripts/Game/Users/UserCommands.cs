using System.Linq;
using Game.Users.Requests;
using IngameDebugConsole;
using UnityEngine;

namespace Game.Users
{
    public static class UserCommands
    {
        [ConsoleMethod("login", "authorization", "username", "password")]
        public static async void Login(string username, string password)
        {
            var result = await UserHelper
                .Service
                .Login(new LoginRequest(username, password));
            
            Debug.Log($"Authorization is completed. Response: {result.AccessToken} (token), {result.TokenType} (token type)");
        }
        
        [ConsoleMethod("register", "registers new user", "username", "password")]
        public static async void Register(string username, string password)
        {
            var result = await UserHelper
                .Service
                .Register(new RegisterRequest(username, string.Empty, password, password));
            
            Debug.Log($"Registration is completed. Response: {result.Id}, {result.Username}, {string.Join(" ", result.Wallets.Select(value => $"type: {value.BalanceType}, balance: {value.Balance}"))}");
        }
    }
}