using CP04.Services;
using CP4.Services.CountryServices.Responses;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CountryTest
{
    public class CountryMoqTest
    {
        private readonly CountryService _countryService;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly string validCountryName;
        private readonly string invalidCountryName;
        private readonly CountriesResponse validCountryResponse;

        public CountryMoqTest()
        {
            // A - Arrange (Preparação)
            _handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_handlerMock.Object);
            _countryService = new CountryService(httpClient);

            validCountryName = "Brazil";
            invalidCountryName = "Narnia";

            validCountryResponse = new CountriesResponse
            {
                Name = new NameResponse { Common = "Brazil", Official = "Federative Republic of Brazil" },
                Flags = new FlagsResponse { Png = "https://flagcdn.com/br.png", Svg = "https://flagcdn.com/br.svg" }
            };
        }

        [Fact]
        public async Task GetAllCountriesAsync_ReturnsListOfCountries()
        {
            // A - Act (Ação)
            var countries = new List<CountriesResponse> { validCountryResponse };
            var json = JsonConvert.SerializeObject(countries);
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == "https://restcountries.com/v3.1/all?fields=name,flags"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var result = await _countryService.GetAllCountriesAsync();

            // A - Assert (Resultado)
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(validCountryResponse.Name.Common, result.First().Name.Common);
        }

        [Fact]
        public async Task GetCountryByNameAsync_ReturnsCountry_WhenCountryNameIsValid()
        {
            // A - Act (Ação)
            var countries = new List<CountriesResponse> { validCountryResponse };
            var json = JsonConvert.SerializeObject(countries);
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == $"https://restcountries.com/v3.1/name/{validCountryName}?fields=name,flags"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var result = await _countryService.GetCountryByNameAsync(validCountryName);

            // A - Assert (Resultado)
            Assert.NotNull(result);
            Assert.Equal(validCountryResponse.Name.Common, result?.Name.Common);
            Assert.Equal(validCountryResponse.Flags.Png, result?.Flags.Png);
        }

        [Fact]
        public async Task GetCountryByNameAsync_ReturnsNull_WhenCountryNameIsInvalid()
        {
            // A - Act (Ação)
            var httpResponse = new HttpResponseMessage(HttpStatusCode.NotFound);

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == $"https://restcountries.com/v3.1/name/{invalidCountryName}?fields=name,flags"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var result = await _countryService.GetCountryByNameAsync(invalidCountryName);

            // A - Assert (Resultado)
            Assert.Null(result);
        }
    }
}
