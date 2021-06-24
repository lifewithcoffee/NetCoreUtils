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
        public IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// This method will update ServiceProvide as well.
        /// </summary>
        public T GetServiceNewScope<T>()
        {
            this.ServiceProvider = this.ServiceProvider.CreateScope().ServiceProvider;
            return this.ServiceProvider.GetService<T>();
        }

        public ServiceProviderFixture()
        {
            //string projectDir = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            //string sharpMemberDir = Path.Combine(projectDir, "../SharpMember");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                //.SetBasePath(sharpMemberDir)
                .AddJsonFile(TestGlobalSettings.JsonSetting, optional: true, reloadOnChange: true)
                .AddJsonFile(TestGlobalSettings.JsonSettingForUnitTest, optional: true, reloadOnChange: true)
                //.AddUserSecrets(userSecretsId: "aspnet-SharpMember-4C3332C6-4145-4408-BDD4-63A97039ED0D")
                .Build();

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(c => c.AddDebug()); // or use mock: serviceCollection.AddTransient<ILogger>(f => new Mock<ILogger>().Object);
            //serviceCollection.AddTransient<ICommunityTestDataProvider, CommunityTestDataProvider>();

            this.ServiceProvider = serviceCollection.BuildServiceProvider();
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
