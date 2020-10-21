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
using NetCoreUtils.Database.MongoDb;
using NetCoreUtils.Database.InfluxDb;

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
                s.AddMongoDb(new MongoDbSetting { DatabaseName = "rltestdb" });
                s.AddInfluxDb(new InfluxDbSetting
                {
                    Token = "4R1aL7t1hZolnMQezXQxkhhMGlqYUBy7g5Ue8RQAQ9wHn_XIHJN_2EpFqaYcD9F2wv_lt-kHqP8Ym99c7Gv5pw=="
                });
            });
        }
    }
}
