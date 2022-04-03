using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCoreUtils.Database.XunitTest;
using Xunit;

namespace DatabaseLibTests
{
    public class RepositoryLibTest : IClassFixture<ServiceProviderFixture>
    {
        ServiceProviderFixture _provider;

        public RepositoryLibTest(ServiceProviderFixture provider)
        {
            _provider = provider;
        }

        [Fact]
        public void DI_should_work()
        {
            var uow = _provider.GetServiceNewScope<IUnitOfWork>();
            Assert.NotNull(uow);
            Assert.NotNull(uow.Context);

            //uow.Context.Database.MigrateAsync();
        }

        [Fact]
        public void MultiTenant_CRUD_should_work()
        {
            // arrange
            var before = _provider.GetServiceNewScope<IRepositoryRead<Project>>();
            Assert.Equal(0, before.QueryAll().Count());

            // act
            var writer = _provider.GetServiceNewScope<IRepositoryWrite<Project>>();
            writer.Add(new Project { Name = "project1" });
            writer.Add(new Project { Name = "project2" });
            writer.Commit();

            // assert

            // If use sqlite non-shared in-memory db, need to call
            // _provider.GetServiceExistingScope<>() instead;
            var reader = _provider.GetServiceNewScope<IRepositoryRead<Project>>();    

            var all = reader.QueryAll().ToList();
            Assert.Equal(2, all.Count);
            Assert.Equal("project1", all[0].Name);
            Assert.Equal("project2", all[1].Name);

            Assert.Equal("tenant1", all[0].TenantId);
            Assert.Equal("tenant1", all[1].TenantId);
        }
    }
}
