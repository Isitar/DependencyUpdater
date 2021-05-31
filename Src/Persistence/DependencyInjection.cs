namespace Isitar.DependencyUpdater.Persistence
{
    using Application.Common.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<IDbContext, DependencyUpdaterDbContext>();
            return services;
        }
    }
}