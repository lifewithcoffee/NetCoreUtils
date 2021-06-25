using Microsoft.Extensions.Configuration;
using NetCoreUtils.Database.XunitTest;
using System;
using Xunit;

namespace BasicTests
{
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

    public class ServiceProviderTest : IClassFixture<ServiceProviderFixture>
    {
        ServiceProviderFixture _provider;

        public ServiceProviderTest(ServiceProviderFixture provider)
        {
            _provider = provider;
        }

        [Fact]
        public void ServiceProviderFixture_ShallNot_Null()
        {
            Assert.NotNull(_provider);
        }

        [Fact]
        public void JsonConfig_Shall_Work()
        {
            var configTest = _provider.GetServiceNewScope<IConfigTest>();

            Assert.Equal("Host=localhost;Database=xUnit;Port=5432;Username=postgres;Password=open", configTest.GetConnectionString("PostgresConnection"));
            Assert.True(configTest.GetBoolValue("TestBoolValueTrue"));
            Assert.False(configTest.GetBoolValue("TestBoolValueFalse"));
        }
    }
}
