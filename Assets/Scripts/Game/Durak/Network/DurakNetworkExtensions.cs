using Newtonsoft.Json;

namespace Game.Durak.Network
{
    public static class DurakNetworkExtensions
    {
        public static string ExtractContent(this DurakConfiguration self)
        {
            var result = JsonConvert.SerializeObject(self);
            return result;
        }
    }
}