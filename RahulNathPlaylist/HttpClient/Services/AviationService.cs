using System.Net.Http;
using System.Threading.Tasks;
using HttpClient.Interfaces;

namespace HttpClient.Services
{
    public class AviationService : IAviationService
    {
        private readonly System.Net.Http.HttpClient _httpClient;

        public AviationService(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Get(string flightStatus)
        {
            try
            {
                var path = $"flights?limit=5&access_key=5520383b1f288d46587c1d481ab2fb08&flight_status={flightStatus}";
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