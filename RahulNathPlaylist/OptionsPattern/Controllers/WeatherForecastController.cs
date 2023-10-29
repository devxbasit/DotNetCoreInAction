using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OptionsPattern.Models;

namespace OptionsPattern.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherApiOptions _weatherApiOptionsSingleton;
        private readonly WeatherApiOptions _weatherApiOptionsScoped;

        public WeatherForecastController(IOptions<WeatherApiOptions> weatherApiOptionsSingleton,
            IOptionsSnapshot<WeatherApiOptions> weatherApiOptionsScoped)
        {
            _weatherApiOptionsSingleton = weatherApiOptionsSingleton.Value;
            _weatherApiOptionsScoped = weatherApiOptionsScoped.Value;
        }

        [HttpGet]
        public string Get()
        {
            var result = "";
            result += $"Url = {_weatherApiOptionsSingleton.Url}, ApiKey = {_weatherApiOptionsSingleton.ApiKey} \n";
            result += $"Url = {_weatherApiOptionsScoped.Url}, ApiKey = {_weatherApiOptionsScoped.ApiKey}";
            return result;
        }
    }
}