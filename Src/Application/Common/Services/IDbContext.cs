namespace Isitar.DependencyUpdater.Application.Common.Services
{
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public interface IDbContext
    {
        public DatabaseFacade Database { get; }
        
        public DbSet<Project> Projects { get; }
        public DbSet<Platform> Platforms { get; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

}