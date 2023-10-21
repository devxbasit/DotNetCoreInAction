using Microsoft.AspNetCore.Mvc;

namespace Routing.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class CurrencyConversionRateController : ControllerBase
    {
        public int Get()
        {
            return 10;
        }
    }
}