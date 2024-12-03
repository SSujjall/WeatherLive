using ISO3166;
using WeatherLive.API.Models.ResponseModels;

namespace WeatherLive.API.Services
{
    public  class GeolocationHelperService
    {
        public string GetNumericCode(string countryName)
        {
            var country = Country.List.FirstOrDefault(c => c.Name.Equals(countryName, StringComparison.OrdinalIgnoreCase));
            if (country == null)
            {
                return "country not found";
            }

            return country.NumericCode.ToString();
        }
    }
}
