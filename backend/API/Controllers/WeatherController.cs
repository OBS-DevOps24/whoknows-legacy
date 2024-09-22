using API.Interfaces;
using API.Models;
using API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/weather")]
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly IGeocodingService _geocodingService;
        public WeatherController(IWeatherService weatherService, IGeocodingService geocodingService)
        {
            _weatherService = weatherService;
            _geocodingService = geocodingService;
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

        //[HttpGet("city")]
        //public async Task<ActionResult<WeatherDTO>> GetWeatherFromCityAndCountry([FromQuery] string city = "copenhagen", [FromQuery] string country = "Denmark")
        //{
        //    var results = await _weatherService.GetWeatherFromCityAndCountry(city, country);
        //    if (results == null)
        //    {
        //        return BadRequest();
        //    }

        //    return Ok(results);
        //}

    }
}
