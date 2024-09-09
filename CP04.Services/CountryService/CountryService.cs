


using CP4.Services.CountryServices.Responses;
using Newtonsoft.Json;


namespace CP04.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;

        public CountryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CountriesResponse>> GetAllCountriesAsync()
        {
            var response = await _httpClient.GetAsync("https://restcountries.com/v3.1/all?fields=name,flags");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var countries = JsonConvert.DeserializeObject<IEnumerable<CountriesResponse>>(content);
                return countries ?? Enumerable.Empty<CountriesResponse>();
            }

            return Enumerable.Empty<CountriesResponse>();
        }

        public async Task<CountriesResponse?> GetCountryByNameAsync(string countryName)
        {
            var response = await _httpClient.GetAsync($"https://restcountries.com/v3.1/name/{countryName}?fields=name,flags");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var countries = JsonConvert.DeserializeObject<IEnumerable<CountriesResponse>>(content);
                return countries?.FirstOrDefault();
            }

            return null;
        }
    }
}
