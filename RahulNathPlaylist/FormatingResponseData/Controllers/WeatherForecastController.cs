using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FormatingResponseData.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        // https://localhost:5001/WeatherForecast
        [HttpGet]
        public IActionResult Get()
        {
            var rng = new Random();
            return new OkObjectResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray());
        }

        // https://localhost:5001/WeatherForecast/Data/json
        // https://localhost:5001/WeatherForecast/Data/xml
        [HttpGet("Data/{format}")]
        [FormatFilter]
        public IActionResult Data()
        {
            var rng = new Random();
            return new OkObjectResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray());
        }

        [HttpGet("Text")]
        public string Text()
        {
            return "Hello";
        }

        [HttpGet("bool")]
        public bool JsonContentTypeInHeader()
        {
            return true;
        }

        [HttpGet("ContentNegotiation")]
        public IActionResult ContentNegotiation()
        {
            var rng = new Random();
            return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray());
        }


        [HttpGet("NoContentNegotiation")]
        public IActionResult NoContentNegotiation()
        {
            var rng = new Random();
            return new JsonResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray());
        }

        [Produces("application/xml")]
        [HttpGet("NoContentNegotiation2")]
        public IActionResult NoContentNegotiation2()
        {
            var rng = new Random();
            return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray());
        }
    }
}