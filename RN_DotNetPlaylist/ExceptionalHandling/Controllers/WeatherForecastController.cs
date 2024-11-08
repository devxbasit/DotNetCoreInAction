using System.Collections.Generic;
using System.Linq;
using ExceptionalHandling.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExceptionalHandling.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        // https://localhost:5001/WeatherForecast/london
        // https://localhost:5001/WeatherForecast/lo

        [HttpGet("{cityName}")]
        public ActionResult<IEnumerable<WeatherForecast>> Get(string cityName)
        {
            return _weatherService.Get(cityName).ToArray();
        }
    }
}