namespace Isitar.DependencyUpdater.Application.Common.Services
{
    using System.Data;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public interface IDbContext
    {
        public DatabaseFacade Database { get; }
        
        public DbSet<Project> Projects { get; }
        public DbSet<Platform> Platforms { get; }
    }

}