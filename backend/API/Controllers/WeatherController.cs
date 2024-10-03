using API.Interfaces;
using API.Models;
using API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet(Name = "GetWeather")]
         public async Task<ActionResult<WeatherDTO?>> GetWeatherData([FromQuery] double? latitude, [FromQuery] double? longitude, [FromQuery] string? city, [FromQuery] string? country)
        {
           
            var results = await _weatherService.GetWeatherData(latitude, longitude, city, country);

            if (results == null)
            {
                return BadRequest();
            }

            return Ok(results);
        }
    }
}
