namespace WeatherLive.API.Models.ResponseModels
{
    public class CommonResponseModel
    {
        public object? Response { get; set; }
    }

    public class CommonGeoLocationResponseModel
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }

    public class CommonWeatherResponseModel
    {
        public string Name { get; set; }
        public string Weather { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string WindSpeed { get; set; }
    }
}
