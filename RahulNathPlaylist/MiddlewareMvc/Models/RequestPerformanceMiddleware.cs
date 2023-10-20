using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MiddlewareMvc.Models
{
    public class RequestPerformanceMiddleware : IMiddleware
    {

        private readonly ILogger<RequestPerformanceMiddleware> _logger;
        public RequestPerformanceMiddleware(ILogger<RequestPerformanceMiddleware> logger)
        {
            _logger = logger;
            
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            
            var stopWatch = new Stopwatch();
            
            stopWatch.Start();

            await next!.Invoke(context);

            stopWatch.Stop();

            if (stopWatch.Elapsed.TotalMilliseconds < 200) return;

            _logger.LogWarning("The Request {request} took more than 200 milliseconds", context.Request.Path);

        }
    }
}