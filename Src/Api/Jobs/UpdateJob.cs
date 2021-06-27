namespace Isitar.DependencyUpdater.Api.Jobs
{
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Common.Services;
    using Application.DependencyMgt.Commands.UpdateProjectDependency;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Quartz;

    public class UpdateJob : IJob
    {
        private readonly IDbContext dbContext;
        private readonly IMediator mediator;

        public UpdateJob(IDbContext dbContext, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var cancellationToken = context.CancellationToken;
            var projectsToUpdate = await dbContext.Projects
                .Where(p => p.CheckRequested && !p.IsChecking)
                .ToListAsync(cancellationToken);
            foreach (var project in projectsToUpdate)
            {
                project.IsChecking = true;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            try
            {
                foreach (var project in projectsToUpdate.AsParallel())
                {
                    await mediator.Send(new UpdateProjectDependencyCommand {ProjectId = project.Id}, cancellationToken);
                    var projectUpdated = await dbContext.Projects.FindAsync(new object[] {project.Id}, cancellationToken);
                    projectUpdated.IsChecking = false;
                    projectUpdated.CheckRequested = false;
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
            catch
            {
                foreach (var project in await dbContext.Projects
                    .Where(p => projectsToUpdate.Select(ptu => ptu.Id).Contains(p.Id))
                    .Where(p => p.IsChecking)
                    .ToListAsync(cancellationToken))
                {
                    project.IsChecking = false;
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}