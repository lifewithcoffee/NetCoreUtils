using NetCoreUtils.Database.XunitTest.TestEnv;
using System;
using Xunit;

namespace NetCoreUtils.Database.XunitTest
{
    public class UnitTest1 : IClassFixture<ServiceProviderFixture>
    {
        ServiceProviderFixture _provider;

        public UnitTest1(ServiceProviderFixture provider)
        {
            _provider = provider;
        }

        [Fact]
        public void Test1()
        {
            Assert.NotNull(_provider);
        }
    }
}
