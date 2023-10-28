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

                var httpClient = _httpClientFactory.CreateClient();
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
                // do not dispose HttpClient or use using statement block
                // https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
                // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-7.0
                // https://stackoverflow.com/questions/15705092/do-httpclient-and-httpclienthandler-have-to-be-disposed-between-requests

                var path = "flights?limit=5&access_key=5520383b1f288d46587c1d481ab2fb08";
                var httpClient = _httpClientFactory.CreateClient("AviationStackHttpClient");
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