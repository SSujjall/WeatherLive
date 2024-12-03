using ISO3166;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using WeatherLive.API.Models.ResponseModels;

namespace WeatherLive.API.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private string geoBaseUrl = "http://api.openweathermap.org/geo/1.0/direct";
        private string weatherBaseUrl = "https://api.openweathermap.org/data/2.5/weather";
        private readonly GeolocationHelperService _geoHelper;
        private readonly IConfiguration _configuration;

        //https://openweathermap.org/current for more info on the weather API

        public WeatherService(HttpClient httpClient, GeolocationHelperService geoHelper, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _geoHelper = geoHelper;
            _configuration = configuration;
        }

        #region Getting long lat using geolocation, then getting weather using that long lat
        public List<string> CountryNames()
        {
            var countryNames = new List<string>();

            foreach (var country in Country.List)
            {
                countryNames.Add(country.Name);
            }

            return countryNames;
        }

        public async Task<CommonResponseModel> GetGeoLocationCordinates(string countryName, string cityName)
        {
            var responseModel = new CommonResponseModel();

            var countryCode = _geoHelper.GetNumericCode(countryName);
            var apiKey = _configuration["OpenWeatherMap:ApiKey"];

            var endPoint = $"{geoBaseUrl}?q={cityName},{countryCode}&limit=1&appid={apiKey}";

            var response = await _httpClient.GetAsync(endPoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var geoLocation = JsonConvert.DeserializeObject<List<GeoLocationResponseModel>>(content);

                #region Model Mapping
                var commonResponseModel = new CommonGeoLocationResponseModel
                {
                    Name = geoLocation.First().name,
                    Latitude = geoLocation.First().lat,
                    Longitude = geoLocation.First().lon,
                    Country = geoLocation.First().country,
                    State = geoLocation.First().state
                };
                #endregion

                responseModel.Response = commonResponseModel; // get the first item in the list
            }
            return responseModel;
        }

        public async Task<CommonResponseModel> GetWeatherByLatLong(string countryName, string cityName)
        {
            var responseModel = new CommonResponseModel();

            var geoLocation = await GetGeoLocationCordinates(countryName, cityName);

            if (geoLocation.Response != null)
            {
                var geoLocationResponse = geoLocation.Response as CommonGeoLocationResponseModel;

                var latitude = geoLocationResponse.Latitude;
                var longitude = geoLocationResponse.Longitude;

                var apiKey = _configuration["OpenWeatherMap:ApiKey"];

                var endPoint = $"{weatherBaseUrl}?lat={latitude}&lon={longitude}&appid={apiKey}";

                var response = await _httpClient.GetAsync(endPoint);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var weather = JsonConvert.DeserializeObject<WeatherResponseModel>(content);

                    #region Model Mapping
                    var commonResponseModel = new CommonWeatherResponseModel
                    {
                        Name = weather.name,
                        Weather = weather.weather.First().description,
                        Temperature = weather.main.temp.ToString(),
                        Humidity = weather.main.humidity.ToString(),
                        WindSpeed = weather.wind.speed.ToString()
                    };
                    #endregion

                    responseModel.Response = commonResponseModel;
                }
            }
            return responseModel;
        }
        #endregion

        #region Directly getting the weather from provided name
        public async Task<CommonResponseModel> GetWeatherByName(string name)
        {
            var responseModel = new CommonResponseModel();

            var apiKey = _configuration["OpenWeatherMap:ApiKey"];
            var units = "metric";

            var endPoint = $"{weatherBaseUrl}?units={units}&q={name}&appid={apiKey}";

            var response = await _httpClient.GetAsync(endPoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var weather = JsonConvert.DeserializeObject<WeatherResponseModel>(content);

                #region Model Mapping
                var commonResponseModel = new CommonWeatherResponseModel
                {
                    Name = weather.name,
                    Weather = weather.weather.First().description,
                    Temperature = Math.Ceiling(weather.main.temp).ToString(),
                    Humidity = weather.main.humidity.ToString(),
                    WindSpeed = weather.wind.speed.ToString()
                };
                #endregion

                responseModel.Response = commonResponseModel;
            }
            return responseModel;
        }
        #endregion
    }
}
