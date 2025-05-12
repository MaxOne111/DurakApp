using System;
using System.Net.Http;

namespace Infrastructure
{
    public static class WebHelper
    {
        private static readonly Lazy<HttpClient> LazyClient 
            = new Lazy<HttpClient>(CreateClient);

        public static HttpClient Client => LazyClient.Value;

        private static HttpClient CreateClient()
        {
            var result = new HttpClient();
            return result;
        }
    }
}