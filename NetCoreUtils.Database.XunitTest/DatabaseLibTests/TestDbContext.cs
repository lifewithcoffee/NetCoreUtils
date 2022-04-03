using Microsoft.EntityFrameworkCore;
using NetCoreUtils.Database.MultiTenancy;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseLibTests
{
    public class TaskItem : TenantEntity
    {
        public int Id { get; set; }
        public string Descriptiont { get; set; }
    }

    public class Project : TenantEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }

    //public class TestDbContext : DbContext
    public class TestDbContext : MultiTenantContext
    {
        //public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        public TestDbContext([NotNull] DbContextOptions options, ITenantProvider tenantProvider) : base(options, tenantProvider)
        {
            Database.EnsureCreated();
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
    }

    public class TenantProvider : ITenantProvider
    {
        public string GetTenantId()
        {
            return "tenant1";
        }
    }
}
