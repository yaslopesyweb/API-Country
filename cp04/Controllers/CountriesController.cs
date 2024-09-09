
using CP04.Services;
using Microsoft.AspNetCore.Mvc;


namespace CP4.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return Ok(countries);
        }

        [HttpGet("{countryName}")]
        public async Task<IActionResult> GetCountryByName(string countryName)
        {
            var country = await _countryService.GetCountryByNameAsync(countryName);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }
    }
}
