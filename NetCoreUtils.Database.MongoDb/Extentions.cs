﻿using Microsoft.Extensions.DependencyInjection;
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

    static public class Extentions
    {
        static public void AddMongoDb(this IServiceCollection services, MongoDbSetting setting)
        {
            services.AddSingleton<IMongoDatabase>(x => {
                var client = new MongoClient($"mongodb://{setting.Host}:{setting.Port}");
                return client.GetDatabase(setting.DatabaseName);
            });
        }
    }
}