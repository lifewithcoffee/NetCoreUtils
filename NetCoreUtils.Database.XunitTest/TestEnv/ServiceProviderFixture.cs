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
            //string projectDir = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            //string sharpMemberDir = Path.Combine(projectDir, "../SharpMember");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                //.SetBasePath(sharpMemberDir)
                .AddJsonFile(_jsonSetting, optional: true, reloadOnChange: true)        // allowed to call this method multiple times to add more json files
                //.AddUserSecrets(userSecretsId: "aspnet-SharpMember-4C3332C6-4145-4408-BDD4-63A97039ED0D")
                .Build();

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(c => c.AddDebug()); // or use moq mock: serviceCollection.AddTransient<ILogger>(f => new Mock<ILogger>().Object);
            //serviceCollection.AddTransient<ICommunityTestDataProvider, CommunityTestDataProvider>();

            this._serviceProvider = serviceCollection.BuildServiceProvider();
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
