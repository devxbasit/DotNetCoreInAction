using System.Net;
using Contracts;
using Entities.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;

namespace LoggingWebApi;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogManager _logger;

    public GlobalExceptionHandler(ILogManager logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

        if (contextFeature != null)
        {
            _logger.LogError($"Something went wrong: ${exception.Message}");

            await httpContext.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Internal Server Error."
            }.ToString());
        }

        //return "true" to state that the execution in the pipeline is over
        return true;
    }
}