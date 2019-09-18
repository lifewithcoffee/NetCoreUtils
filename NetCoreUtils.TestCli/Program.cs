using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using CoreCmd.CommandExecution;
using NetCoreUtils.TestCli.HostedServices;

namespace NetCoreUtils.TestCli
{
    class DefaultCommand
    {
        public void TestLogging()
        {
            var builder = new HostBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddEnvironmentVariables();

                //if (args != null)
                //    config.AddCommandLine(args);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddOptions();
                //services.Configure<DaemonConfig>(hostContext.Configuration.GetSection("Daemon"));
                services.AddHostedService<LifetimeEventsHostedService>();
                services.AddHostedService<TimedHostedService>();
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
            });

            builder.RunConsoleAsync().Wait();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            new AssemblyCommandExecutor().Execute(args);
        }
    }
}
