using System;
using System.Net.Http;
using System.Threading.Tasks;
using HttpClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HttpClient.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly AviationStackOptions _aviationStackOptions;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IOptions<AviationStackOptions> aviationStackOptions,
            ILogger<WeatherForecastController> logger)
        {
            _aviationStackOptions = aviationStackOptions.Value;
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> GetData()
        {
            var url = new Uri(
                $"{_aviationStackOptions.Protocol}{_aviationStackOptions.Domain}/v1/flights?access_key={_aviationStackOptions.ApiKey}");

            try
            {
                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(100, ex, "Http Request Error");
                return ex.Message;
            }
        }
    }
}