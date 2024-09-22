﻿using API.Controllers;

namespace API.Models
{
    public class WeatherResponse
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double GenerationTime_Ms { get; set; }
        public int Utc_Offset_Seconds { get; set; }
        public string Timezone { get; set; }
        public string Timezone_Abbreviation { get; set; }
        public double Elevation { get; set; }
        public CurrentUnits Current_Units { get; set; }
        public CurrentWeather Current { get; set; }
    }

    public class CurrentUnits
    {
        public string Time { get; set; }
        public string Interval { get; set; }
        public string Temperature_2m { get; set; }
        public string Wind_Speed_10m { get; set; }
    }

    public class CurrentWeather
    {
        public string Time { get; set; }
        public int Interval { get; set; }
        public double Temperature_2m { get; set; }
        public double Wind_Speed_10m { get; set; }
    }
}
