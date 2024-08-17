using System.Net.Sockets;
using Microsoft.AspNetCore.Identity;

namespace MVCApp.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            // policy 1
            options.AddPolicy("AllowAllPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            } );
            // policy 2
            options.AddPolicy("CustomPolicy", builder =>
            {
                builder.AllowAnyMethod().WithMethods("GET");
            } );
        });
    }

    public static void ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
        {
            options.Cookie.Name = "MyCookieAuth";
            options.LoginPath = "/SignIn";
            options.LogoutPath = "/SignOut";
            options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
        });
    }

    public static void ConfigureAuthorization(this IServiceCollection services)
    {

    }
}
