namespace NugetUpdater
{
    using Isitar.DependencyUpdater.Application.Common.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddNugetUpdater(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IProjectUpdater, NugetProjectUpdater>();
            return services;
        }
    }
}