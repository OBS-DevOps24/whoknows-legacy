using System.Text.Json.Serialization;

namespace API.Models
{
    public class GeocodingResponse
    {
        [JsonPropertyName("results")]
        public List<Geocoding> Results { get; set; }
    }

    public class  Geocoding
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}
