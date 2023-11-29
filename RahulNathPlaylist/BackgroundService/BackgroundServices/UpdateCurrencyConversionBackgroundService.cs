using System;
using System.Threading;
using System.Threading.Tasks;
using BackgroundTask.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundTask.BackgroundServices
{
    public class UpdateCurrencyConversionBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UpdateCurrencyConversionBackgroundService> _logger;
        
        public UpdateCurrencyConversionBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<UpdateCurrencyConversionBackgroundService> logger
        )
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    _logger.LogInformation("Updating Currency Conversion at {datetime}", DateTime.Now);
                    var scopedService = scope.ServiceProvider.GetRequiredService<IScopedService>();
                    scopedService.Write();
                    await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                }
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Started At - {datetime}", DateTime.Now);
            return base.StartAsync(cancellationToken);
        }


        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Stopped At - {datetime}",
                DateTime.Now);
            return base.StopAsync(cancellationToken);
        }
    }
}