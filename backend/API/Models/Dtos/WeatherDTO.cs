namespace API.Models.Dtos
{
    public class WeatherDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Time { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string TemperatureUnit { get; set; }
        public double TemperatureValue { get; set; }
        public string WindSpeedUnit { get; set; }
        public double WindSpeedValue { get; set; }
    }
}
