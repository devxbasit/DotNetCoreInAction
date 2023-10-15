using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Configuration
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
                // override the default AppConfiguration
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var dict = new Dictionary<string, string>
                    {
                        { "Message2", "Value from Dictionary" }
                    };

                    // adding order matters below, same key will get overridden
                    builder.AddJsonFile("MyConfigAppSettings.json");
                    builder.AddInMemoryCollection(dict);
                });
    }
}