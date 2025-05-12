namespace Utils
{
    public static class JsonHelper
    {
        public static bool TryDeserialize<T>(this string data, out T result)
        {
            result = default;
            
            try
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
                return true;    
            }
            catch
            {
                return false;
            }
        }
    }
}