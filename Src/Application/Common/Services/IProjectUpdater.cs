namespace Isitar.DependencyUpdater.Application.Common.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IProjectUpdater
    {
        public ProjectType SupportedProjectType { get; }
        public string WorkingDirectory { get; set; }
        public Task<IEnumerable<string>> UpdateProjectDependenciesAsync(Project project, string workingDirectory, CancellationToken cancellationToken);
    }
}