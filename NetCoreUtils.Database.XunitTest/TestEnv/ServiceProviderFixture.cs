using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace NetCoreUtils.Database.XunitTest.TestEnv
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

            this._serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Dispose() { }
    }

    interface IConfigTest
    {
        bool GetBoolValue(string key);
        string GetConnectionString(string name);
    }

    class ConfigTest : IConfigTest
    {
        private readonly IConfiguration _configuration;

        public ConfigTest(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public bool GetBoolValue(string key)
        {
            return _configuration.GetValue<bool>(key);
        }

        public string GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name);
        }
    }

    /**
     * Use the above fixture in a collection if needed:
     * 
     * [CollectionDefinition(nameof(ServiceProviderCollection))]
     * public class ServiceProviderCollection : ICollectionFixture<ServiceProviderFixture> { }
     */
}
