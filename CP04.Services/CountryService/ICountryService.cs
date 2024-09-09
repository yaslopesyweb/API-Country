using CP4.Services.CountryServices.Responses;
using System.Diagnostics.Metrics;

namespace CP04.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<CountriesResponse>> GetAllCountriesAsync();
        Task<CountriesResponse?> GetCountryByNameAsync(string countryName);
    }
}
