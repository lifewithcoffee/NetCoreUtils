using CoreCmd.CommandExecution;
using NetCoreUtils.Database.InfluxDb;
using NetCoreUtils.Database.MongoDb;
using System;
using System.Threading.Tasks;

namespace TestApp.Cli.Database
{
    class Program
    {
        static async Task Main(string[] args)
        {
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
