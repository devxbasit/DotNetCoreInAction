using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HttpClient.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FlightsController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FlightsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // https: //localhost:5001/Flights/Get
        public async Task<string> Get()
        {
            var url = new Uri(
                "http://api.aviationstack.com/v1/flights?limit=5&access_key=5520383b1f288d46587c1d481ab2fb08");

            try
            {
                // ***
                // using var httpClient = new System.Net.Http.HttpClient(); // bad practice, create new socket connection everytime

                using var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
            }
        }
        

        // https://localhost:5001/Flights/GetFromNamedClient
        public async Task<string> GetFromNamedClient()
        {
            try
            {
                var path = "flights?limit=5&access_key=5520383b1f288d46587c1d481ab2fb08";
                using var httpClient = _httpClientFactory.CreateClient("AviationStackHttpClient");
                var response = await httpClient.GetAsync(path);
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