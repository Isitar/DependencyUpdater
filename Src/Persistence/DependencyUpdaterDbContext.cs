namespace Isitar.DependencyUpdater.Persistence
{
    using System;
    using System.IO;
    using Application.Common.Services;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class DependencyUpdaterDbContext : DbContext, IDbContext
    {
        private readonly DatabaseSettings databaseSettings;

        public DependencyUpdaterDbContext(DatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.GetFullPath(databaseSettings.Location);
            var dbDirectory = Path.GetDirectoryName(dbPath);
            if (!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }
            if (Directory.Exists(dbPath))
            {
                dbPath = Path.Combine(dbPath, "dependency-updater.db");
            }
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Platform> Platforms { get; set; }
    }
}