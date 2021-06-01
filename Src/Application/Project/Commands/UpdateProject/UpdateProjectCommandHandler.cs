namespace Isitar.DependencyUpdater.Application.Project.Commands.UpdateProject
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Common.Services;
    using MediatR;

    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
    {
        private readonly IDbContext dbContext;
        private readonly IPublisher publisher;

        public UpdateProjectCommandHandler(IDbContext dbContext, IPublisher publisher)
        {
            this.dbContext = dbContext;
            this.publisher = publisher;
        }

        public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FindAsync(new object[] {request.Id}, cancellationToken);
            project.Name = request.Name.TrimToNull();
            project.ProjectType = request.ProjectType;
            project.UpdateFrequency = request.UpdateFrequency.TrimToNull();
            project.TargetBranch = request.TargetBranch.TrimToNull();
            project.PlatformProjectId = request.PlatformProjectId.TrimToNull();
            project.Url = request.Url.TrimToNull();
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}