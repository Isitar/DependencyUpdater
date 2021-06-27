namespace Isitar.DependencyUpdater.Api.Jobs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Common.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Quartz;

    public class MarkingJob : IJob
    {
        private readonly IDbContext dbContext;
        private readonly ILogger<MarkingJob> logger;

        public MarkingJob(IDbContext dbContext, ILogger<MarkingJob> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var now = DateTimeOffset.Now;
            now = now.Subtract(TimeSpan.FromSeconds(now.Second));
            now = now.Subtract(TimeSpan.FromMilliseconds(now.Millisecond));
            foreach (var project in await dbContext.Projects.Where(p => !p.IsChecking && !p.CheckRequested).ToListAsync(context.CancellationToken))
            {
                if (new CronExpression(project.UpdateFrequency).IsSatisfiedBy(now))
                {
                    logger.LogTrace("marking project {id} {name} for update", project.Id, project.Name);
                    project.CheckRequested = true;
                }
            }

            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}