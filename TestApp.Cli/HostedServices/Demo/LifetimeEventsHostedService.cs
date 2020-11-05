using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreUtils.TestCli.HostedServices.Demo
{
    class LifetimeEventsHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IApplicationLifetime _appLifetime;
        private Timer _timer;

        public LifetimeEventsHostedService(
            ILogger<LifetimeEventsHostedService> logger,
            IApplicationLifetime appLifetime)
        {
            _logger = logger;
            _appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            // terminate service after several seconds
            int count = 5;
            _timer = new Timer(state => {
                _logger.LogInformation($"count = {count}");
                if(--count < 0)
                {
                    _appLifetime.StopApplication();
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");

            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");

            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");

            // Perform post-stopped activities here
        }
    }
}
