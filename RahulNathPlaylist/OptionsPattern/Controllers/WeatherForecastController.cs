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
        private readonly WeatherApiOptions _config1, _config2, _config3;

        public WeatherForecastController(IOptions<WeatherApiOptions> config1,
            IOptionsSnapshot<WeatherApiOptions> config2,
            IOptionsMonitor<WeatherApiOptions> config3)
        {
            try
            {
                _config1 = config1.Value;
                _config2 = config2.Value;
                _config3 = config3.CurrentValue;

                // register callback 
                var disposable = config3.OnChange((weatherApiOptions, name) =>
                {
                    Console.WriteLine($"url = {weatherApiOptions.Url}, ApiKey = {weatherApiOptions.ApiKey}");
                });

                // stop listening to changes
                disposable.Dispose();
            }
            catch (OptionsValidationException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public string Get()
        {
            var result = "";
            result += $"Url = {_config1.Url}, ApiKey = {_config1.ApiKey} \n";
            result += $"Url = {_config2.Url}, ApiKey = {_config2.ApiKey}\n";
            result += $"Url = {_config3.Url}, ApiKey = {_config3.ApiKey}";
            return result;
        }
    }
}