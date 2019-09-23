using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace NetCoreUtils.TestCli.HostedServices
{
    class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private IConfiguration _config;
        private NetCoreUtilsOptions _option;

        public TimedHostedService(ILogger<TimedHostedService> logger, IConfiguration config, IOptionsMonitor<NetCoreUtilsOptions> option, NetCoreUtilsOptions optionInst)
        {
            _logger = logger;
            _config = config;

            // for IConfiguration config
            var val1 = _config.GetValue<string>("NetCoreUtils:TestConfig");
            var val2 = _config["NetCoreUtils:TestConfig"];
            var val3 = _config.GetSection("NetCoreUtils").GetValue<string>("TestConfig");

            var ncuconfig = new NetCoreUtilsOptions();
            _config.GetSection("NetCoreUtils").Bind(ncuconfig);
            var val4 = ncuconfig.TestConfig;

            var val5 = _config.GetSection("NetCoreUtils").Get<NetCoreUtilsOptions>().TestConfig;

            // for IOptionsMonitor<NetCoreUtilsOptions> option
            _option = option.CurrentValue;
            var val6 = _option.TestConfig;

            // for NetCoreUtilsOptions optionInst
            var val7 = optionInst.TestConfig;

            // ===================
            if (config == null)
                logger.LogError("config is null");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(3));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
