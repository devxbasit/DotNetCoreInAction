using System;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HttpClient.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FlightsController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AviationStackOptions _aviationStackOptions;

        public FlightsController(IHttpClientFactory httpClientFactory,
            IOptionsSnapshot<AviationStackOptions> aviationStackOptions)
        {
            _httpClientFactory = httpClientFactory;
            _aviationStackOptions = aviationStackOptions?.Value;
        }

        // https: //localhost:5001/Flights/Get
        public async Task<string> Get()
        {
            var url = new Uri(
                $"http://api.aviationstack.com/v1/flights?limit=5&access_key={_aviationStackOptions.ApiKey}");

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

                var path = $"flights?limit=5&access_key={_aviationStackOptions.ApiKey}";
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