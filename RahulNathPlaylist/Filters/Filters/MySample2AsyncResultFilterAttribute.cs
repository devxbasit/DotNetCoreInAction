using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Filters.Filters
{
    public class MySample2AsyncResultFilterAttribute : Attribute, IAsyncResultFilter
    {
        private readonly ILogger<MySample2AsyncResultFilterAttribute> _logger;
        private readonly string _name;
        private readonly Guid _randomId;

        // type filter support constructor dependencies provided from DI, can also manually provide some other dependencies
        public MySample2AsyncResultFilterAttribute(
            ILogger<MySample2AsyncResultFilterAttribute> logger,
            string name = "Global")
        {
            _logger = logger;
            _name = name;
            _randomId = Guid.NewGuid();
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            _logger.LogInformation($"ResultFilter2 Async Before - {_name} {_randomId}");
            await next();
            _logger.LogInformation($"ResultFilter2 Async Before - {_name} {_randomId}");
        }
    }
}