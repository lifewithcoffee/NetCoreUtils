using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DatabaseLibTests
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Descriptiont { get; set; }
    }

    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
