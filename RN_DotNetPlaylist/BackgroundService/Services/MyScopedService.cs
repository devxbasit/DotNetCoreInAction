using System;
using Microsoft.Extensions.Logging;

namespace BackgroundTask.Services
{
    public class ScopedService : IScopedService
    {
        private readonly ILogger<ScopedService> _logger;
        private readonly Guid _randomId;

        public ScopedService(ILogger<ScopedService> logger)
        {
            _logger = logger;
            _randomId = Guid.NewGuid();
        }

        public void Write()
        {
            _logger.LogInformation($"Scoped service writing... {_randomId}");
        }
    }

    public interface IScopedService
    {
        void Write();
    }
}