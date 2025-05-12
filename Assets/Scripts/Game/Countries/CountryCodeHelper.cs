using System;
using System.Collections.Generic;
using Game.Countries.Enums;

namespace Game.Countries
{
    public static class CountryCodeHelper
    {
        private static readonly Lazy<IDictionary<string, ECountry>> LazyCountries =
            new Lazy<IDictionary<string, ECountry>>(CreateCountriesDictionary);

        public static bool TryGetCountry(string code, out ECountry result)
        {
            var hasCountry = LazyCountries.Value.TryGetValue(code, out var country);
            result = hasCountry ? country : ECountry.Unknown;
            
            return hasCountry;
        }

        private static IDictionary<string, ECountry> CreateCountriesDictionary()
        {
            var result = new Dictionary<string, ECountry>()
            {
                ["AZ"] = ECountry.Azerbaijan,
                ["AM"] = ECountry.Armenia,
                ["BY"] = ECountry.Belarus,
                ["KZ"] = ECountry.Kazakhstan,
                ["KG"] = ECountry.Kyrgyzstan,
                ["MD"] = ECountry.Moldova,
                ["RU"] = ECountry.Russia,
                ["TJ"] = ECountry.Tajikistan,
                ["TM"] = ECountry.Turkmenistan,
                ["UZ"] = ECountry.Uzbekistan,
                ["UA"] = ECountry.Ukraine,
                ["MN"] = ECountry.Mongolia,
                ["CN"] = ECountry.China,
                ["US"] = ECountry.Usa,
            };

            return result;
        }
    }
}