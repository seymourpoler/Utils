using System;
using System.Data.Entity;
using CodeFirstSample.Entities;
using System.Data.Entity.Infrastructure;

namespace CodeFirstSample
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base() { }
        public DataBaseContext(string connectionString) : base(connectionString) { }

        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Status> Status{ get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().ToTable("Groups");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Task>().ToTable("Tasks");
            modelBuilder.Entity<Status>().ToTable("Status");
            modelBuilder.Entity<Company>().ToTable("Companies");

            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
