namespace Isitar.DependencyUpdater.Git
{
    using Application.Common.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddGit(this IServiceCollection services, IConfiguration configuration)
        {
            var gitSettings = new GitSettings();
            configuration.Bind(nameof(GitSettings), gitSettings);
            services.AddSingleton(gitSettings);
            services.AddTransient<IGitService, GitService>();


            return services;
        }
    }
}