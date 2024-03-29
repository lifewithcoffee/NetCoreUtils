﻿using NetCoreUtils.Database;
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
        public async Task MultiTenant_CRUD_should_work()
        {
            // arrange
            var before = _provider.GetServiceNewScope<IRepository<Project>>();
            before.RemoveAll();
            before.Commit();
            Assert.Equal(0, before.QueryAll().Count());

            // act
            var repo1 = _provider.GetServiceNewScope<IRepository<Project>>();
            var proj = new Project { Name = "project1" };
            repo1.Add(proj);
            repo1.Add(new Project { Name = "project2" });
            repo1.Commit();

            var repo2 = _provider.GetServiceNewScope<IRepository<Project>>();
            var loadedProj = await repo2.GetAsync(proj.Id);
            loadedProj.Name = "updated_name";
            repo2.Commit();

            // assert

            // If use sqlite non-shared in-memory db, need to call
            // _provider.GetServiceExistingScope<>() instead;
            var reader = _provider.GetServiceNewScope<IRepositoryReadonly<Project>>();    

            var all = reader.QueryAll().ToList();
            Assert.Equal(2, all.Count);
            Assert.Equal("project2", all[0].Name);
            Assert.Equal("updated_name", all[1].Name);

            Assert.Equal("tenant1", all[0].TenantId);
            Assert.Equal("tenant1", all[1].TenantId);
        }
    }
}
