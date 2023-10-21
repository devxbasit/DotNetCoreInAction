using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Routing.RouteFilters;

namespace Routing
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
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Routing", Version = "v1" }); });

            services.AddHealthChecks();
            services.AddRouting(options =>
            {
                options.ConstraintMap.Add("startsWithZero", typeof(StartsWithZeroRouteConstraint));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Routing v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                var endPoint = context.GetEndpoint();
                await next();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // https://localhost:5001/Hello
                endpoints.MapGet("/Hello", async context => { await context.Response.WriteAsync("Hello"); });

                // https://localhost:5001/health
                endpoints.MapHealthChecks("/health");
                // endpoints.MapHealthChecks("/health").RequireAuthorization();

                // https://localhost:5001/customer/1234567812345678/statement
                endpoints.MapGet("/customer/{customerAccountNumber:long:length(16)}/statement", async context =>
                {
                    var customerAccountNumber = context.GetRouteValue("customerAccountNumber");
                    await context.Response.WriteAsync($"AccountNumber = {customerAccountNumber}");
                });

                // https://localhost:5001/TestConstraint/1234567812345678/0Basit
                // https://localhost:5001/TestConstraint/1234567812345678/Basit
                endpoints.MapGet("/TestConstraint/{AccountNumber:long}/{name:startsWithZero}",
                    async context => { await context.Response.WriteAsync("Constraint Passed"); });

                // https://localhost:5001/ambiguous
                endpoints.MapGet("/ambiguous", async context => { await context.Response.WriteAsync("Hello"); });
                endpoints.MapGet("/ambiguous", async context => { await context.Response.WriteAsync("Hello"); });

                endpoints.MapControllers();
            });
        }
    }
}