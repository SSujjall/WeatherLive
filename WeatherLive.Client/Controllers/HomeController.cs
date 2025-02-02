using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherLive.Client.Models;

namespace WeatherLive.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly WeatherConfig _weatherConfig;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient, WeatherConfig weatherConfig)
        {
            _logger = logger;
            _httpClient = httpClient;
            _weatherConfig = weatherConfig;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public Task<ActionResult> GetWeather(string countryName)
        {
            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
