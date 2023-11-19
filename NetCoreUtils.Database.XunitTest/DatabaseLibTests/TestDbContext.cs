using Microsoft.EntityFrameworkCore;
using NetCoreUtils.Database.MultiTenant;
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

    public class TestDbContext : DbContext
    {
        public TestDbContext()
        {
        }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=xUnit;Port=5432;Username=postgres;Password=open");
            //Database.EnsureCreated();
            base.OnConfiguring(optionsBuilder);
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
