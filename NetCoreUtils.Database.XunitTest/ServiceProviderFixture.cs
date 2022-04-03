using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreUtils.Database;
using BasicTests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using DatabaseLibTests;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace NetCoreUtils.Database.XunitTest
{
    public class ServiceProviderFixture : IDisposable
    {
        string _jsonSetting = "appsettings.json";

        IServiceProvider _serviceProvider;

        public T GetServiceExistingScope<T>()
        {
            return this._serviceProvider.GetService<T>();
        }

        /// <summary>
        /// This method will update ServiceProvide as well.
        /// </summary>
        public T GetServiceNewScope<T>()
        {
            this._serviceProvider = this._serviceProvider.CreateScope().ServiceProvider;
            return this._serviceProvider.GetService<T>();
        }

        public ServiceProviderFixture()
        {
            string projectDir = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            string settingFileDir = Path.Combine(projectDir, ".");

            /**
             * - If need to add more json files, call .AddJsonFile() multiple times
             * - If need to apply secrets, apply .AddUserSecrets(userSecretsId: "<secret-id>") before .Build()
             */
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(settingFileDir)
                .AddJsonFile(_jsonSetting, optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(c => c.AddDebug()); // or use moq mock: serviceCollection.AddTransient<ILogger>(f => new Mock<ILogger>().Object);
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddTransient<IConfigTest, ConfigTest>();

            /**
             * Or use EF InMemory DB:
             * serviceCollection.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("xUnit"));
             */
            serviceCollection.AddDbContext<TestDbContext>(options => options.UseSqlite(CreateSqliteInMemoryDatabase()));

            serviceCollection.AddRepositories<TestDbContext>(new TenantProvider());

            this._serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private DbConnection CreateSqliteInMemoryDatabase()
        {
            // non-shared in-memory db, a new scope DbContext opens a different
            // connection:
            // var connection = new SqliteConnection("Filename=:memory:"); 

            // shared in-memory db, a new scope DbContext reuse the same
            // connection if the previous connection hasn't broken
            var connection = new SqliteConnection("DataSource=myshareddb;mode=memory;cache=shared");

            connection.Open();  // EF core will close it when DbContext disposes
            return connection;
        }

        public void Dispose() { }
    }

    /**
     * Use the above fixture in a collection if needed:
     * 
     * [CollectionDefinition(nameof(ServiceProviderCollection))]
     * public class ServiceProviderCollection : ICollectionFixture<ServiceProviderFixture> { }
     */
}
