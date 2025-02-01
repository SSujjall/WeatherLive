using Microsoft.AspNetCore.Mvc;
using WeatherLive.API.Models.ResponseModels;
using WeatherLive.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(); // Needed for http client used in weather service
builder.Services.AddScoped<GeolocationHelperService>();
builder.Services.AddScoped<WeatherService>();

var app = builder.Build();



#region minimal api to get country code from country name using ISO3166 package
app.MapGet("/get-numeric-code-for-testing", async (string countryName, GeolocationHelperService _service) =>
{
    var numericCode = _service.GetNumericCode(countryName);
    return Results.Ok(new CommonResponseModel // common response model for clarity
    {
        Response = new
        {
            CountryName = countryName,
            NumericCode = numericCode
        }
    });
});
#endregion

#region minimal api to get geolocation(lat, long) using country name
app.MapGet("/get-geo-for-testing", async (string countryName, string cityName, WeatherService _service) =>
{
    var geoLocation = await _service.GetGeoLocationCordinates(countryName, cityName);
    return Results.Ok(geoLocation);
});
#endregion

#region minimal api to get all coountry names.
app.MapGet("/get-all-countries", async (WeatherService _service) =>
{
    var countryNames = _service.CountryNames(); 
    return Results.Ok(countryNames);
});
#endregion

#region minimal api to get weather using lat long
app.MapGet("/get-weather-lat-long", async (string countryName, string cityName, WeatherService _service) =>
{
    var result = await _service.GetWeatherByLatLong(countryName, cityName);
    return Results.Ok(result);
});
#endregion

#region minimal api to get weather directly from name
app.MapGet("/get-weather-main", async (string name, WeatherService _service) =>
{
    var result = await _service.GetWeatherByName(name);
    return Results.Ok(result);
});
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
