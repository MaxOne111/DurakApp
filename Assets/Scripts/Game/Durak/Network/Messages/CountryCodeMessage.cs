using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Messages
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class CountryCodeMessage
        : DurakMessageBase
    {
        [JsonConstructor]
        public CountryCodeMessage
        (
            [JsonProperty(PropertyName = "mode")] ETurnMode mode,
            [JsonProperty(PropertyName = "user_id")] int userId,
            [JsonProperty(PropertyName = "code")] string code
        ) : base(mode)
        {
            UserId = userId;
            Code = code;
        }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId
        {
            get;
        }

        [JsonProperty(PropertyName = "code")]
        public string Code
        {
            get;
        }
    }
}