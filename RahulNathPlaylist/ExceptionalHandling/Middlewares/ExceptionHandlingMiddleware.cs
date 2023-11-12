using System;
using System.Threading.Tasks;
using ExceptionalHandling.ExceptionalHandling.Domain.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExceptionalHandling.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DomainValidationException ex)
            {
                _logger.LogInformation(ex.Message);
                context.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (DomainNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                context.Response.StatusCode = (int)StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                context.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}