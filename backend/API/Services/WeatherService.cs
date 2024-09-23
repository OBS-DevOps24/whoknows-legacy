using API.Interfaces;
using API.Models;
using API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace API.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _factory;
        private readonly string _baseURL = "https://api.open-meteo.com";
        private readonly IGeocodingService _geocodingService;
        public WeatherService(IHttpClientFactory factory, IGeocodingService geocodingService) 
        {
            _factory = factory;
            _geocodingService = geocodingService;
        }

        public async Task<WeatherDTO> GetWeatherData(double? latitude, double? longitude, string? city, string? country)
        {
            if (IsCityProvided(city))
            {
                return await GetWeatherByCityAndCountry(city, country);
            }

            if (IsCoordinatesValid(latitude, longitude))
            {
                return await GetWeatherByCoordinates(latitude.Value, longitude.Value);
            }

            return await GetWeatherByCityAndCountry("Copenhagen", "Denmark");

        }

        private bool IsCoordinatesValid(double? latitude, double? longitude)
        {
            return latitude.HasValue && longitude.HasValue &&
                   (latitude.Value >= -90 && latitude.Value <= 90) &&
                   (longitude.Value >= -180 && longitude.Value <= 180);
        }

        private bool IsCityProvided(string? city)
        {
            return !string.IsNullOrWhiteSpace(city);
        }

        private async Task<WeatherDTO> GetWeatherByCoordinates(double latitude, double longitude)
        {
            var client = _factory.CreateClient();

            client.BaseAddress = new Uri(_baseURL);
            WeatherResponse? weatherRepsonse = await client.GetFromJsonAsync<WeatherResponse>($"v1/forecast?latitude={latitude.ToString(CultureInfo.InvariantCulture)}&longitude={longitude.ToString(CultureInfo.InvariantCulture)}&current=temperature_2m,wind_speed_10m");

            if (weatherRepsonse == null)
            {
                return null;
            }

            return new WeatherDTO
            {
                Latitude = weatherRepsonse.Latitude,
                Longitude = weatherRepsonse.Longitude,
                Time = weatherRepsonse.Current.Time,
                Country = "",
                City = "",
                TemperatureUnit = weatherRepsonse.Current_Units.Temperature_2m,
                TemperatureValue = weatherRepsonse.Current.Temperature_2m,
                WindSpeedUnit = weatherRepsonse.Current_Units.Wind_Speed_10m,
                WindSpeedValue = weatherRepsonse.Current.Wind_Speed_10m
            };
        }

        private async Task<WeatherDTO> GetWeatherByCityAndCountry(string city, string country)
        {
            var geocoding = await _geocodingService.GetGeocodingData(city, country);
            if (geocoding == null)
            {
                return null;
            }

            var weatherData = await GetWeatherByCoordinates(geocoding.Latitude, geocoding.Longitude);
            if (weatherData == null)
            {
                return null;
            }

            weatherData.Country = geocoding.Country;
            weatherData.City = geocoding.Name;

            return weatherData;
        }
    }
}
