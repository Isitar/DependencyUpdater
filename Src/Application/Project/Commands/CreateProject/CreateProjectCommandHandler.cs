namespace Isitar.DependencyUpdater.Application.Project.Commands.CreateProject
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Common.Services;
    using Domain.Entities;
    using MediatR;

    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand>
    {
        private readonly IDbContext dbContext;

        public CreateProjectCommandHandler(IDbContext dbContext, IPublisher publisher)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            await dbContext.Projects.AddAsync(new Project
            {
                Id = request.Id,
                Name = request.Name.TrimToNull(),
                PlatformId = request.PlatformId,
                ProjectType = request.ProjectType,
                UpdateFrequency = "0 0 0 * *",
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}