using Filters.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Filters.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureFilter(this IServiceCollection services)
        {
            services.AddSingleton(typeof(MySampleAsyncResultFilterAttribute));

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new MySampleActionFilterAttribute("Global Sync"));
                options.Filters.Add(new MySampleAsyncActionFilterAttribute("Global Async", 1));
                options.Filters.Add(new MySampleAsyncResourceFilterAttribute("Global Async"));
                options.Filters.Add(new MySampleAsyncAuthorizationFilterAttribute("Global Async"));

                // service filter
                options.Filters.AddService<MySampleAsyncResultFilterAttribute>();

                // typed filter
                options.Filters.Add<MySample2AsyncResultFilterAttribute>();
            });
        }
    }
}