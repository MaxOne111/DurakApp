using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game.Users.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class RegisterResponse
    {
        [JsonConstructor]
        public RegisterResponse
        (
            [JsonProperty(PropertyName = "id")]
            int id,
            
            [JsonProperty(PropertyName = "username")]
            string username,
            
            [JsonProperty(PropertyName = "avatar")]
            string avatar,
            
            [JsonProperty(PropertyName = "wallets")]
            WalletInfo[] wallets
        )
        {
            Id = id;
            Username = username;
            Avatar = avatar;
            Wallets = wallets;
        }

        public int Id
        {
            get;
        }

        public string Username
        {
            get;
        }

        public string Avatar
        {
            get;
        }

        public IReadOnlyList<WalletInfo> Wallets
        {
            get;
        }
    }
}