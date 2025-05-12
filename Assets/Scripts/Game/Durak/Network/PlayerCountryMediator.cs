using System;
using System.Collections.Generic;
using Game.Countries.Enums;

namespace Game.Durak.Network
{
    public static class PlayerCountryMediator
    {
        private static readonly Lazy<IDictionary<int, ECountry>> LazyPlayerCountries 
            = new Lazy<IDictionary<int, ECountry>>(() => new Dictionary<int, ECountry>());

        public static IReadOnlyDictionary<int, ECountry> PlayerCountries 
            => (IReadOnlyDictionary<int, ECountry>) LazyPlayerCountries.Value;

        public static void AddCountry(int playerId, ECountry country)
        {
            LazyPlayerCountries.Value[playerId] = country;
        }
    }
}