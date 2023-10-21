using Microsoft.AspNetCore.Mvc;

namespace Routing.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/{apiToken:int}")]
    public class CurrencyConversionRateController
    {
        [HttpGet, HttpPost]
        [Route("get/{provider:alpha}")]
        public string Get(int apiToken, string provider)
        {
            // https://localhost:5001/api/v1/CurrencyConversionRate/123456789/get/fixer
            return $"api token = {apiToken}, provider = {provider}";
        }
    }
}