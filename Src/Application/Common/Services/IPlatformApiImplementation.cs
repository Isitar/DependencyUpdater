namespace Isitar.DependencyUpdater.Application.Common.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IPlatformApiImplementation
    {
        public PlatformType SupportedPlatformType { get; }
        public Task CreateOrUpdateMergeRequestAsync(Project project, string sourceBranch, string targetBranch, string title, string message, CancellationToken cancellationToken);
    }
}