using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MiddlewareMvc.Models
{
    public class ConsoleLoggerMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("Logging...");
            next(context);
            return Task.CompletedTask;
        }
    }
}
