using NetCoreUtils.Database.XunitTest.TestEnv;
using System;
using Xunit;

namespace NetCoreUtils.Database.XunitTest.Tests
{
    public class UnitTest1 : IClassFixture<ServiceProviderFixture>
    {
        ServiceProviderFixture _provider;

        public UnitTest1(ServiceProviderFixture provider)
        {
            _provider = provider;
        }

        [Fact]
        public void ServiceProviderFixture_ShallNot_Null()
        {
            Assert.NotNull(_provider);
        }

        [Fact]
        public void Test_JsonConfig()
        {
            var configTest = _provider.GetServiceNewScope<IConfigTest>();

            Assert.Equal("Host=localhost;Database=xUnit;Port=5432;Username=postgres;Password=open", configTest.GetConnectionString("PostgresConnection"));
            Assert.True(configTest.GetBoolValue("TestBoolValueTrue"));
            Assert.False(configTest.GetBoolValue("TestBoolValueFalse"));
        }
    }
}
