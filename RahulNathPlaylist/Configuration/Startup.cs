using System;
using Configuration.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Configuration
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Configuration", Version = "v1" });
            });

            services.Configure<AviationStackOptions>(Configuration.GetSection(nameof(AviationStackOptions)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                // Default IConfiguration source precedence   
                // appSetting.json > appSetting.development.json > User Secrets > Environment Variable > Command Line > and then Custom Added AppConfigurations (like json file, in-memory dictionary)

                Console.WriteLine("------------------------------------------------From appSettings");
                Console.WriteLine($"KeyInBothAppSettingsFile value = {Configuration["KeyInBothAppSettingsFile"]}");
                Console.WriteLine(
                    $"Default Log Level = {Configuration.GetSection("Logging:LogLevel").GetValue<string>("Default")}");
                Console.WriteLine($"fallback value {Configuration.GetValue<int>("InvalidKey", 22)}");

                var aviationStackOptions = new AviationStackOptions();
                Configuration.GetSection(nameof(AviationStackOptions)).Bind(aviationStackOptions);

                var option = Configuration.GetSection(nameof(AviationStackOptions)).Get<AviationStackOptions>();

                Console.WriteLine($"AviationStack UserName = {aviationStackOptions.UserName}");
                Console.WriteLine($"AviationStack ApiKey = {option.ApiKey}");

                Console.WriteLine("------------------------------------------------ From environment variable - linux");
                Console.WriteLine($"Linux $PATH  = {Configuration.GetValue<string>("HOME")}");

                Console.WriteLine("------------------------------------------------ From dotnet user secret manager");
                // run below command in specific project folder 
                //dotnet user-secrets init 
                //dotnet user-secrets set "Admin:SensitiveInfo" "Some Sensitive Info"
                Console.WriteLine(
                    $"Secrets Manager - Admin:SensitiveInfo, {Configuration.GetSection("Admin").GetValue<string>("SensitiveInfo")}");


                Console.WriteLine("------------------------------------------------ From Custom Json File");
                Console.WriteLine($"Message1 =  {Configuration.GetValue<string>("Message1")}");

                Console.WriteLine("------------------------------------------------ From In Memory Dictionary");
                Console.WriteLine($"Message2 =  {Configuration.GetValue<string>("Message2")}");


                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Configuration v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}