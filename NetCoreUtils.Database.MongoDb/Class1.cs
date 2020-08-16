using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace NetCoreUtils.Database.MongoDb
{
    public class MongoDbSetting
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 27017;
    }

    static public class Class1
    {
        static public void AddMongoDb(this IServiceCollection services, MongoDbSetting setting)
        {
            services.AddSingleton<MongoClient>(x => new MongoClient($"mongodb://{setting.Host}:{setting.Port}"));
        }
    }
}
