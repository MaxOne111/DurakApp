using UnityEngine;

namespace Game.Countries
{
    public sealed class CountriesTest
        : MonoBehaviour
    {
        private void Start()
        {
            var provider = new IpCountryProvider();
            var country = provider.GetCountry();
            
           Debug.Log(country); 
        }
    }
}