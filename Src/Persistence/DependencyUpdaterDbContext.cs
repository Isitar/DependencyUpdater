namespace Isitar.DependencyUpdater.Persistence
{
    using System;
    using Application.Common.Services;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class DependencyUpdaterDbContext : DbContext, IDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(nameof(DependencyUpdaterDbContext));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Platform> Platforms { get; set; }
    }
}