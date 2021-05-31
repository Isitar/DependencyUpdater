namespace Isitar.DependencyUpdater.GitLab
{
    using Application.Common.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddGitlab(this IServiceCollection services)
        {
            services.AddScoped<IPlatformApiImplementation, GitLabApiImplementation>();
            return services;
        }
    }
}