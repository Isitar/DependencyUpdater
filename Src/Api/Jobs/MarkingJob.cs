namespace Isitar.DependencyUpdater.Api.Jobs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Common.Services;
    using Microsoft.EntityFrameworkCore;
    using Quartz;

    public class MarkingJob : IJob
    {
        private readonly IDbContext dbContext;

        public MarkingJob(IDbContext dbContext, ISchedulerFactory schedulerFactory)
        {
            this.dbContext = dbContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            foreach (var project in await dbContext.Projects.Where(p => !p.IsChecking && !p.CheckRequested).ToListAsync(context.CancellationToken))
            {
                if (new CronExpression(project.UpdateFrequency).IsSatisfiedBy(DateTimeOffset.Now))
                {
                    project.CheckRequested = true;
                }
            }

            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}