namespace Isitar.DependencyUpdater.Persistence
{
    using Application.Common.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = new DatabaseSettings();
            configuration.Bind(nameof(DatabaseSettings), databaseSettings);
            services.AddSingleton(databaseSettings);

            services.AddDbContext<IDbContext, DependencyUpdaterDbContext>();
            
            return services;
        }
    }
}