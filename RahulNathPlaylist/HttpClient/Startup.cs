using System;
using System.Net.Mime;
using HttpClient.Interfaces;
using HttpClient.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace HttpClient
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HttpClient", Version = "v1" });
            });

            // commands for Linux
            // $ ping api.example.com
            // $ host api.example.com
            // $ sudo netstat -ap | grep 2606:4700:3032

            services.AddHttpClient();
            services.AddHttpClient("AviationStackHttpClient", client =>
            {
                client.BaseAddress = new Uri("http://api.aviationstack.com/v1/");
                client.Timeout = TimeSpan.FromSeconds(20);
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, MediaTypeNames.Application.Xml);
            });

            services.AddHttpClient<IAviationService, AviationService>(client =>
            {
                client.BaseAddress = new Uri("http://api.aviationstack.com/v1/");
                client.Timeout = TimeSpan.FromSeconds(20);
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, MediaTypeNames.Application.Xml);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HttpClient v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}