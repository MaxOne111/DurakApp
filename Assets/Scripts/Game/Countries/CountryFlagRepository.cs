using System;
using System.Collections.Generic;
using System.Linq;
using Game.Countries.Enums;
using UnityEngine;

namespace Game.Countries
{
    [CreateAssetMenu(menuName = "Game/Countries/CountryFlagRepository", fileName = "CountryFlagRepository")]
    public sealed class CountryFlagRepository
        : ScriptableObject
    {
        [SerializeField] private List<CountryFlagPair> pairs;

        public Sprite GetFlag(ECountry country)
        {
            var result = pairs.First(value => value.Country == country).Flag;
            return result;
        }
        
        [Serializable]
        private struct CountryFlagPair
        {
            [field: SerializeField]
            public ECountry Country
            {
                get;
                private set;
            }

            [field: SerializeField]
            public Sprite Flag
            {
                get;
                private set;
            }
        }
    }
}