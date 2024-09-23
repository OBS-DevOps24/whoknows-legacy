using API.Models;
using API.Models.Dtos;

namespace API.Interfaces
{
    public interface IGeocodingService
    {
        Task<Geocoding> GetGeocodingData(string city, string country);
    }
}
