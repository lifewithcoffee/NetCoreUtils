using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using CoreCmd.CommandExecution;
using System.IO;
using Serilog;
using Serilog.Events;
using System.Threading.Tasks;

namespace NetCoreUtils.TestCli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            await new AssemblyCommandExecutor().ExecuteAsync(args, s =>
            {
            });
        }
    }
}
