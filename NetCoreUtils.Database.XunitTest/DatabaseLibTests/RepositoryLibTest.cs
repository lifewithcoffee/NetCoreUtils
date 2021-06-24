using Microsoft.EntityFrameworkCore;
using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;
using Xunit;

namespace DatabaseLibTests
{
    public class TestDbContext : DbContext
    { }

    public class RepositoryLibTest : IClassFixture<ServiceProviderFixture>
    {
        ServiceProviderFixture _provider;

        public RepositoryLibTest(ServiceProviderFixture provider)
        {
            _provider = provider;
        }

        [Fact]
        public void DI_Shall_Work()
        {
            Assert.NotNull(_provider.GetServiceNewScope<IUnitOfWork<TestDbContext>>());
        }
    }
}
