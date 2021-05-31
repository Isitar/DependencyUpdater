namespace Isitar.DependencyUpdater.Process
{
    using Application.Common.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddProcess(this IServiceCollection services)
        {
            services.AddSingleton<IProcessExecutor, ProcessExecutor>();
            return services;
        }
    }
}