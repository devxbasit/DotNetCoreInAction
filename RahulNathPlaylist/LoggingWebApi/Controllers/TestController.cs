using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoggingWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public void Get()
        {
            _logger.LogInformation(1, "Test Info Log");
            _logger.LogWarning(2, "Test warning Log");
            _logger.LogCritical(3, "Test Critical Log");
        }

        [HttpGet]
        [Route("Hello")]
        public void Hello()
        {
            using (_logger.BeginScope(new Dictionary<string, object>()
                   {
                       { "PersonId", 5 },
                       { "TransactionId", 1099 }
                   }))
            {
                _logger.LogCritical(1, "Hello");
            }

            _logger.LogCritical(1, "World");
        }
    }
}