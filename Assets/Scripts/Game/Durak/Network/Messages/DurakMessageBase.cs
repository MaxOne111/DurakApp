using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak.Network.Messages
{
    public abstract class DurakMessageBase
    {
        protected DurakMessageBase(ETurnMode mode)
        {
            Mode = mode;
        }
        
        [JsonProperty(PropertyName = "mode", Order = -2)]
        public ETurnMode Mode
        {
            get;
        }
    }
}