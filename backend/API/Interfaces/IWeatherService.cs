using API.Models;
using API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherDTO> GetWeatherData(double? latitude, double? longitude, string? city, string? country);
    }
}
