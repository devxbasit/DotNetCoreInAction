using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polly.Interfaces;

namespace Polly.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<FlightsController> _logger;
        private readonly IAviationService _aviationService;

        public FlightsController(ILogger<FlightsController> logger, IAviationService aviationService)
        {
            _logger = logger;
            _aviationService = aviationService;
        }

        [HttpGet]
        public async Task<string> Get(string flightStatus)
        {
            return await _aviationService.GetFlights(flightStatus);
        }
    }
}