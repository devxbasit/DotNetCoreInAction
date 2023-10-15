using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiddlewareMvc.Models;


namespace MiddlewareMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddTransient<ConsoleLoggerMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Map("/favicon.ico", (_) => { });

            app.UseMiddleware<ConsoleLoggerMiddleware>();

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Hello Use(). ");
                await next();
            });

            app.UseWhen((context) =>
            {
                if (context.Request.Query.ContainsKey("useWhen"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }, (applicationBuilder) =>
            {
                applicationBuilder.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync("Hello UseWhen(). ");
                    await next();
                });

                applicationBuilder.Run(async (context) => { await context.Response.WriteAsync("Hello UseWhen(). "); });
            });


            //https://localhost:5001/map1/map2/map3/map4/map5

            app.Map("/map1", (applicationBuilder) =>
            {
                applicationBuilder.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync("Hello Map1()");
                    await next();
                });

                applicationBuilder.Map("/map2/map3", (applicationBuilder) =>
                {
                    applicationBuilder.Use(async (context, next) =>
                    {
                        await context.Response.WriteAsync("Hello Map2()");
                        await next();
                    });
                });
            });

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Bye.");
                await next();
            });

            app.Run(async (context) => { await context.Response.WriteAsync("Hello Run()."); });


            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // else
            // {
            //     app.UseExceptionHandler("/Home/Error");
            //     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //     app.UseHsts();
            // }
            //
            // app.UseHttpsRedirection();
            // app.UseStaticFiles();
            //
            // app.UseRouting();
            //
            // app.UseAuthorization();
            //
            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllerRoute(
            //         name: "default",
            //         pattern: "{controller=Home}/{action=Index}/{id?}");
            // });
        }
    }
}