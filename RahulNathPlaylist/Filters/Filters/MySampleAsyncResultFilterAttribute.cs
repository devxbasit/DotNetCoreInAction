using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Filters.Filters
{
    public class MySampleAsyncResultFilterAttribute : Attribute, IAsyncResultFilter
    {
        private readonly ILogger<MySampleAsyncResultFilterAttribute> _logger;
        private readonly string _name;
        private readonly Guid _randomId;

        // service filter support constructor dependencies provided from DI, but cannot manually provide some other values
        public MySampleAsyncResultFilterAttribute(
            ILogger<MySampleAsyncResultFilterAttribute> logger,
            string name = "Global")
        {
            _logger = logger;
            _name = name;
            _randomId = Guid.NewGuid();
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            _logger.LogInformation($"ResultFilter Async Before - {_name} {_randomId}");
            await next();
            _logger.LogInformation($"ResultFilter Async After - {_name} {_randomId}");
        }
    }
}