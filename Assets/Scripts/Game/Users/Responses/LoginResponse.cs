using Newtonsoft.Json;

namespace Game.Users.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class LoginResponse
    {
        [JsonConstructor]
        public LoginResponse
        (
            [JsonProperty(PropertyName = "access_token")]
            string accessToken,
            
            [JsonProperty(PropertyName = "token_type")]
            string tokenType
        )
        {
            AccessToken = accessToken;
            TokenType = tokenType;
        }
        
        public string AccessToken
        {
            get;
        }

        public string TokenType
        {
            get;
        }
    }
}