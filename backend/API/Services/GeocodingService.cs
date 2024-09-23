using API.Interfaces;
using API.Models;
using System.Globalization;

namespace API.Services
{
    public class GeocodingService : IGeocodingService
    {
        private readonly IHttpClientFactory _factory;
        private readonly string _baseURL = "https://geocoding-api.open-meteo.com";

        public GeocodingService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<Geocoding> GetGeocodingData(string city, string country)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return null;
            }

            var client = _factory.CreateClient();
            client.BaseAddress = new Uri(_baseURL);

            var requestUrl = $"v1/search?name={city}";

            // Fetch the geocoding data
            GeocodingResponse? geocodingResponse = await client.GetFromJsonAsync<GeocodingResponse>(requestUrl);

            // Check for null or empty results
            if (geocodingResponse == null || geocodingResponse.Results == null || geocodingResponse.Results.Count == 0)
            {
                return null;
            }

            // Find the best match based on the provided country

            if (string.IsNullOrWhiteSpace(country))
            {
                return geocodingResponse.Results[0];
            }
            var geocodeWithCityAndCountry = geocodingResponse.Results
                .FirstOrDefault(g => g.Country.Equals(country, StringComparison.OrdinalIgnoreCase));

            // Fallback to the first result if no exact match is found
            if (geocodeWithCityAndCountry == null)
            {
                geocodeWithCityAndCountry = geocodingResponse.Results[0];
            }

            return geocodeWithCityAndCountry;
        }
    }
}
