using System.Net.Http;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.Extensions.Options;
using Polly.Interfaces;

namespace Polly.Services
{
    public class AviationService : IAviationService
    {
        private readonly HttpClient _httpClient;
        private readonly AviationStackOptions _aviationStackOptions;

        public AviationService(HttpClient httpClient,
            IOptionsSnapshot<AviationStackOptions> aviationStackOptions)
        {
            _httpClient = httpClient;
            _aviationStackOptions = aviationStackOptions?.Value;
        }

        public async Task<string> GetFlights(string flightStatus)
        {
            try
            {
                var path = $"flights?limit=5&access_key={_aviationStackOptions.ApiKey}&flight_status={flightStatus}";
                var response = await _httpClient.GetAsync(path);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
            }
        }
    }
}