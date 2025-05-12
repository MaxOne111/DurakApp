using System.Threading.Tasks;
using Game.Countries.Enums;

namespace Game.Countries.Abstractions
{
    public interface ICountryProvider
    {
        ECountry GetCountry();
    }

    public interface IAsyncCountryProvider
    {
        Task<ECountry> GetCountry();
    }
}