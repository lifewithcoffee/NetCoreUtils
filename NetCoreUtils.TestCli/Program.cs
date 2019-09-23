using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using CoreCmd.CommandExecution;
using NetCoreUtils.TestCli.HostedServices;
using Serilog;
using Serilog.Events;
using System.IO;
using System.Configuration;

namespace NetCoreUtils.TestCli
{
    public class NetCoreUtilsOptions
    {
        public string TestConfig { get; set; }
    }

    class DefaultCommand
    {
        /**
         * Use serilog: set UseSerilog to true in the appsettings.Development.json
         * Environment variable prefix: NETCOREUTILS_
         */
        public void HostedService()
        {
            new HostBuilder()
                //.ConfigureContainer<>
                .ConfigureHostConfiguration(config =>
                {
                    config.AddEnvironmentVariables(prefix: "NETCOREUTILS_");
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    Console.WriteLine($"Directory.GetCurrentDirectory() = {Directory.GetCurrentDirectory()}");
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddJsonFile(
                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);

                    //if (args != null)
                    //    config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();

                    // config for options
                    var sec = hostContext.Configuration.GetSection("NetCoreUtils");
                    services.Configure<NetCoreUtilsOptions>(sec);
                    services.AddSingleton(sec.Get<NetCoreUtilsOptions>());

                    // config hosted services
                    services.AddHostedService<LifetimeEventsHostedService>();
                    services.AddHostedService<TimedHostedService>();
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    var useSerilog = hostContext.Configuration.GetValue<bool>("Logging:UseSerilog");
                    if (useSerilog)
                    {
                        Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Information()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .CreateLogger();

                        Log.Information("Serilog initialized");

                        logging.AddSerilog();
                    }
                    else
                    {
                        logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                    }
                })
                //.UseSerilog()
                .Build().Run(); // or: builder.RunConsoleAsync().Wait();
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
