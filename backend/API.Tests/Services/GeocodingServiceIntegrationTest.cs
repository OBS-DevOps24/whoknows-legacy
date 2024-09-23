using API.Interfaces;
using API.Models.Dtos;
using API.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests.Services
{
    public class GeocodingServiceIntegrationTest
    {
        private readonly IGeocodingService _geocodingService;

        public GeocodingServiceIntegrationTest()
        {
            // Setup a real HttpClientFactory
            var services = new ServiceCollection();
            services.AddHttpClient();  // Register IHttpClientFactory

            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            // Instantiate the GeocodingService with the real HttpClientFactory
            _geocodingService = new GeocodingService(httpClientFactory);
        }

        [Fact]
        public async Task GetGeocodingData_ReturnsCorrectData()
        {
            // Arrange
            var city = "Paris";
            var country = "France";

            // Act
            var result = await _geocodingService.GetGeocodingData(city, country);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Paris", result.Name);
            Assert.Equal("France", result.Country);
        }

        [Fact]
        public async Task GetGeocodingData_ReturnsNull_WhenCityNotFound()
        {
            // Arrange
            var city = "UnknownCity";
            var country = "UnknownCountry";

            // Act
            var result = await _geocodingService.GetGeocodingData(city, country);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetGeocodingData_ReturnsCorrectData_WhenUsingPartialSearchOnCity()
        {
            // Arrange
            var city = "Copen";
            var country = "Denmark";

            // Act
            var result = await _geocodingService.GetGeocodingData(city, country);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Copenhagen", result.Name);
            Assert.Equal("Denmark", result.Country);
        }

        [Fact]
        public async Task GetGeocodingData_ReturnsCorrectData_WhenUsingPartialSearchOnCityAndCountry()
        {
            // Arrange
            var city = "Copenhag";
            var country = "";
            // Act
            var result = await _geocodingService.GetGeocodingData(city, country);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Copenhagen", result.Name);
            Assert.Equal("Denmark", result.Country);
        }

        [Fact]
        public async Task GetGeocodingData_ReturnsCorrectData_WhenCountryIsEmpty()
        {
            // Arrange
            var city = "Copenhagen";
            var country = "";
            // Act
            var result = await _geocodingService.GetGeocodingData(city, country);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Copenhagen", result.Name);
            Assert.Equal("Denmark", result.Country);
        }

        [Fact]
        public async Task GetGeocodingData_ReturnsCorrectData_WhenCountryIsNull()
        {
            // Arrange
            var city = "Copenhagen";
            // Act
            var result = await _geocodingService.GetGeocodingData(city, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Copenhagen", result.Name);
            Assert.Equal("Denmark", result.Country);
        }
    }
}
