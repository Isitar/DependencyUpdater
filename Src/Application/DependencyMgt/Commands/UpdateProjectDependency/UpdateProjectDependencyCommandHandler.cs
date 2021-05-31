namespace Isitar.DependencyUpdater.Application.DependencyMgt.Commands.UpdateProjectDependency
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Services;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class UpdateProjectDependencyCommandHandler : IRequestHandler<UpdateProjectDependencyCommand>
    {
        private readonly IDbContext dbContext;
        private readonly IGitService gitService;
        private readonly IEnumerable<IProjectUpdater> projectUpdaters;
        private readonly IEnumerable<IPlatformApiImplementation> platformApiImplementations;
        private readonly ILogger<UpdateProjectDependencyCommandHandler> logger;
        private const string UpdateBranchName = "update/dependency";

        public UpdateProjectDependencyCommandHandler(IDbContext dbContext, IGitService gitService,
            IEnumerable<IProjectUpdater> projectUpdaters,
            IEnumerable<IPlatformApiImplementation> platformApiImplementations,
            ILogger<UpdateProjectDependencyCommandHandler> logger
        )
        {
            this.dbContext = dbContext;
            this.gitService = gitService;
            this.projectUpdaters = projectUpdaters;
            this.platformApiImplementations = platformApiImplementations;
            this.logger = logger;
        }

        public async Task<Unit> Handle(UpdateProjectDependencyCommand request, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.Include(p => p.Platform).Where(p => p.Id.Equals(request.ProjectId)).FirstOrDefaultAsync(cancellationToken);
            var workingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(workingDirectory);
            try
            {
                gitService.WorkingDirectory = workingDirectory;

                await gitService.CloneRepositoryAsync(project.Url, project.Platform, cancellationToken);

                await gitService.CheckoutBranchAsync(project.TargetBranch, false, cancellationToken);

                var existingBranch = await gitService.BranchExistsAsync(UpdateBranchName, cancellationToken);
                if (existingBranch)
                {
                    logger.LogInformation("Update branch already exists, adding updates to it");
                }

                await gitService.CheckoutBranchAsync(UpdateBranchName, !existingBranch, cancellationToken);
                if (existingBranch)
                {
                    try
                    {
                        await gitService.MergeAsync(project.TargetBranch, cancellationToken);
                    }
                    catch
                    {
                        logger.LogError("Merge failed, scrap branch and start new one");
                        await gitService.ResetHardAsync(cancellationToken);
                        await gitService.CheckoutBranchAsync(project.TargetBranch, false, cancellationToken);
                        await gitService.DeleteBranchAsync(UpdateBranchName, true, cancellationToken);
                        await gitService.CheckoutBranchAsync(UpdateBranchName, true, cancellationToken);
                    }
                }

                var versionChanges = new List<string>();
                foreach (var projectUpdater in projectUpdaters.Where(pu => (pu.SupportedProjectType & project.ProjectType) > 0))
                {
                    projectUpdater.WorkingDirectory = workingDirectory;
                    versionChanges.AddRange(await projectUpdater.UpdateProjectDependenciesAsync(project, workingDirectory, cancellationToken));
                }

                // no changes
                if (versionChanges.Count == 0)
                {
                    logger.LogInformation("No updates for project {project}", project.Name);
                    return Unit.Value;
                }

                await gitService.AddFilesAsync(cancellationToken);
                if (versionChanges.Count > 1)
                {
                    versionChanges = versionChanges.Prepend("Dependency Updates").ToList();
                }
                await gitService.CommitAddedFilesAsync(string.Join(Environment.NewLine, versionChanges), cancellationToken);
                await gitService.PushAsync("origin", UpdateBranchName, cancellationToken);

                var platformApi = platformApiImplementations.FirstOrDefault(pai => pai.SupportedPlatformType.Equals(project.Platform.PlatformType));
                if (null == platformApi)
                {
                    logger.LogError("No Api implementation found for platform: {platform}", project.Platform.PlatformType);
                    throw new NotImplementedException($"Platform api not implemented for type {project.Platform.PlatformType}");
                }

                await platformApi.CreateOrUpdateMergeRequestAsync(project, UpdateBranchName, project.TargetBranch, "Dependency Update", string.Join(Environment.NewLine, versionChanges), cancellationToken);
            }
            finally
            {
                Directory.Delete(workingDirectory, true);
            }

            return Unit.Value;
        }
    }
}