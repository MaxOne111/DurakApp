using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.UI
{
    [JsonObject(MemberSerialization.OptIn)]
    public readonly struct ChatMessage
    {
        [JsonConstructor]
        public ChatMessage
        (
            [JsonProperty(PropertyName = "Type")]
            EChatMessageType type,
            
            [JsonProperty(PropertyName = "Id")]
            string text
        )
        {
            Type = type;
            Text = text;
        }

        [JsonProperty(PropertyName = "Type")]
        public EChatMessageType Type
        {
            get;
        }

        [JsonProperty(PropertyName = "Id")]
        public string Text
        {
            get;
        }
    }
}