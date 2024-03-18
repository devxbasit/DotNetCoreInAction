using Contracts;
using LoggerService;
using Microsoft.Net.Http.Headers;

namespace LoggingWebApi.ExtensionMethods;

public static class ServicesExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();

                // builder
                //     .WithOrigins("https://facebook.com", "https://twitter.com")
                //     .WithMethods(HttpMethods.Get, HttpMethods.Post)
                //     .WithHeaders(HeaderNames.Accept, HeaderNames.ContentType, "X-Custom-Header");

            });
        });
    }
    
    public static void ConfigureIISIntegration(this IServiceCollection services)
    {
        services.Configure<IISOptions>(options =>
        {
            // TODO
            
        });
    }
    
    public static void ConfigureLogging(this IServiceCollection services)
    {
        services.AddSingleton<ILogManager, LogManager>();
    }
}
