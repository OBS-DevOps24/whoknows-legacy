using API.Interfaces;
using API.Services;
using Microsoft.Extensions.DependencyInjection;


namespace API.Tests.Services
{
    public class WeatherServiceIntegrationTest
    {
        private readonly IWeatherService _weatherService;
        private readonly IGeocodingService _geocodingService;

        public WeatherServiceIntegrationTest()
        {
            // Setup a real HttpClientFactory
            var services = new ServiceCollection();
            services.AddHttpClient();  // Register IHttpClientFactory

            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            // Instantiate the WeatherService with the real HttpClientFactory
            _geocodingService = new GeocodingService(httpClientFactory);
            _weatherService = new WeatherService(httpClientFactory, _geocodingService);

        }

        [Fact]
        public async Task GetWeatherData_ReturnsCorrectData_WhenLatAndLongAreCorrect()
        {
            // Arrange
            double latitude = 55.6785;
            double longitude = 12.570435;

            // Act
            var result = await _weatherService.GetWeatherData(latitude, longitude,null,null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(latitude, result.Latitude);
            Assert.Equal(longitude, result.Longitude);
        }

        [Fact]
        public async Task GetWeatherData_ReturnsDefaultData_WhenNoParameterIsGiven()
        {
            // Act
            var result = await _weatherService.GetWeatherData(null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Copenhagen", result.City); // Check for default city
            Assert.Equal("Denmark", result.Country); // Check for default country
        }

        [Fact]
        public async Task GetWeatherData_ReturnsWeatherData_WhenCityIsProvided()
        {
            // Arrange
            string city = "Copenhagen";
            string country = "Denmark";

            // Act
            var result = await _weatherService.GetWeatherData(null, null, city, country);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(city, result.City);
            Assert.Equal(country, result.Country);
        }

        [Fact]
        public async Task GetWeatherData_ReturnsDefaultData_WhenInvalidCoordinatesAreProvided()
        {
            // Arrange
            double? invalidLatitude = 100; // Invalid latitude
            double? invalidLongitude = 200; // Invalid longitude

            // Act
            var result = await _weatherService.GetWeatherData(invalidLatitude, invalidLongitude, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Copenhagen", result.City); // Check for default city
            Assert.Equal("Denmark", result.Country); // Check for default country
        }

        [Fact]
        public async Task GetWeatherData_ReturnsWeatherDataForCity_WhenCityAndCoordinatesAreProvided()
        {
            // Arrange
            double latitude = 12;
            double longitude = 13;
            string city = "Copenhagen";
            string country = "Denmark";

            // Act
            var result = await _weatherService.GetWeatherData(latitude, longitude, city, country);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(city, result.City);
            Assert.Equal(country, result.Country);
            Assert.NotEqual(latitude, result.Latitude);
            Assert.NotEqual(longitude, result.Longitude);
        }

        [Fact]
        public async Task GetWeatherData_ReturnsWeatherData_WhenCityAreProvided()
        {
            // Arrange
            string city = "Copenhagen";

            // Act
            var result = await _weatherService.GetWeatherData(null, null, city, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(city, result.City);
            Assert.Equal("Denmark", result.Country);
        }
    }
}
