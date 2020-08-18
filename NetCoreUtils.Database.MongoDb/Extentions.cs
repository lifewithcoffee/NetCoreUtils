using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace NetCoreUtils.Database.MongoDb
{
    public class MongoDbSetting
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 27017;
        public string DatabaseName { get; set; }
    }

    public interface IMongoDbConnection
    {
        public IMongoClient MongoClient { get; }
        public IMongoDatabase MongoDatabase { get; }
    }

    public class MongoDbConnection : IMongoDbConnection
    {
        IMongoClient _mongoClient;
        IMongoDatabase _mongoDatabase;

        public MongoDbConnection(MongoDbSetting setting)
        {
            _mongoClient = new MongoClient($"mongodb://{setting.Host}:{setting.Port}");
            _mongoDatabase = _mongoClient.GetDatabase(setting.DatabaseName);
        }

        public IMongoClient MongoClient => _mongoClient;
        public IMongoDatabase MongoDatabase => _mongoDatabase;
    }

    static public class Extentions
    {
        static public void AddMongoDb(this IServiceCollection services, MongoDbSetting setting)
        {
            services.AddSingleton<IMongoDbConnection>(x => new MongoDbConnection(setting));
            services.AddTransient(typeof(IMongoDocReader<>), typeof(MongoDocReader<>));
            services.AddTransient(typeof(IMongoDocWriter<>), typeof(MongoDocWriter<>));
        }
    }
}
