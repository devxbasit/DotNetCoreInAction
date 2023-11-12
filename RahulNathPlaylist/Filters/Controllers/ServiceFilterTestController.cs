using Filters.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Filters.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(MySampleAsyncResultFilterAttribute))]
    public class ServiceFilterTestController : ControllerBase
    {
        [ServiceFilter(typeof(MySampleAsyncResultFilterAttribute))]
        [HttpGet]
        public string ServiceFilter()
        {
            return "Ok";
        }
    }
}