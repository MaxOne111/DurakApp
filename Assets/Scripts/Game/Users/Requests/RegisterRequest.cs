using Newtonsoft.Json;

namespace Game.Users.Requests
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class RegisterRequest
    {
        public RegisterRequest(string username, string avatar, string password, string passwordRepeat = null)
        {
            passwordRepeat ??= password;
            
            Username = username;
            Avatar = avatar;
            Password = password;
            PasswordRepeat = passwordRepeat;
        }
        
        [JsonProperty(PropertyName = "username")]
        public string Username
        {
            get;
        }
        
        [JsonProperty(PropertyName = "avatar")]
        public string Avatar
        {
            get;
        }
        
        [JsonProperty(PropertyName = "password")]
        public string Password
        {
            get;
        }
        
        [JsonProperty(PropertyName = "password_repeat")]
        public string PasswordRepeat
        {
            get;
        }
        
    }
}