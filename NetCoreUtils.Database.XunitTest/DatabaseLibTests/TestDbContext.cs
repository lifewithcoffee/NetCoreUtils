using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetCoreUtils.Database.MultiTenant;
using NetCoreUtils.Database.XunitTest.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

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
        /**
         * _note_: Must provide a parameterless constructor, and it must be the first constructor,
         * otherwise command "update-command" can't work.
         */
        public TestDbContext() { }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }   // used in DI

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = ConfigUtil.AppSettings.GetConnectionString("PostgresConnection");
            optionsBuilder.UseNpgsql(connStr);
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
