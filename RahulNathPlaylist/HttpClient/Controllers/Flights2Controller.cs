using System.Net.Http;
using System.Threading.Tasks;
using HttpClient.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HttpClient.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Flights2Controller
    {
        private readonly IAviationService _aviationService;

        public Flights2Controller(IAviationService aviationService)
        {
            _aviationService = aviationService;
        }

        // scheduled, active, landed, cancelled, incident, diverted
        // https://localhost:5001/Flights2/GetFromTypedClient?&flightStatus=active
        // https://localhost:5001/Flights2/GetFromTypedClient?&flightStatus=scheduled

        public async Task<string> GetFromTypedClient(string flightStatus)
        {
            try
            {
                return await _aviationService.GetFlights(flightStatus);
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
            }
        }
    }
}