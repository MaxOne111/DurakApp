using Newtonsoft.Json;

namespace Game.Users.Requests
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class LoginRequest
    {
        public LoginRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
        
        [JsonProperty(PropertyName = "username")]
        public string Username
        {
            get;
        }

        [JsonProperty(PropertyName = "password")]
        public string Password
        {
            get;
        }
    }
}