using System.Net;
using System.Net.Http;
using Game.Countries.Abstractions;
using Game.Countries.Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Countries
{
    public sealed class IpCountryProvider
        : ICountryProvider
    {
        private const string GetIpUrl = @"https://api.ipify.org";

        private static string GetIpInfoUrl(string ip)
        {
            var result = $"https://ipapi.co/{ip}/json/";
            return result;
        }
        
        public ECountry GetCountry()
        {
            var ip = new WebClient().DownloadString(GetIpUrl);
            var url = GetIpInfoUrl(ip);

            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var text = response.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<HostData>(text);
            
            Debug.Log(text);

            var canGetCountry = CountryCodeHelper.TryGetCountry(data.CountryCode, out var result);
            if (canGetCountry)
            {
                return result;
            }

            return ECountry.Unknown;
        }

        private struct HostData
        {
            [JsonConstructor]
            public HostData
            (
                [JsonProperty(PropertyName = "continent_code")] string continentCode,
                [JsonProperty(PropertyName = "continent_name")] string continentName,
                [JsonProperty(PropertyName = "country_code")] string countryCode,
                [JsonProperty(PropertyName = "country_name")] string countryName,
                [JsonProperty(PropertyName = "region_code")] string regionCode
            )
            {
                ContinentCode = continentCode;
                ContinentName = continentName;
                CountryCode = countryCode;
                CountryName = countryName;
                RegionCode = regionCode;
            }

            public string ContinentCode
            {
                get;
            }

            public string ContinentName
            {
                get;
            }
            
            public string CountryCode
            {
                get;
            }
            
            public string CountryName
            {
                get;
            }

            public string RegionCode
            {
                get;
            }
        }
    }
}