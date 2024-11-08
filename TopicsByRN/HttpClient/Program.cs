using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HttpClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureAppConfiguration((hostingContext, configBuilder) =>
                {
                    var coreAppPath = Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, "..", "Core");
                    configBuilder.AddJsonFile(Path.Combine(coreAppPath, "coreAppSettings.json"), optional:false, reloadOnChange: true);
                });
    }
}