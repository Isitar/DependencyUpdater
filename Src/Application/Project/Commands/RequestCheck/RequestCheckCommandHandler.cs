namespace Isitar.DependencyUpdater.Application.Project.Commands.RequestCheck
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Services;
    using MediatR;

    public class RequestCheckCommandHandler : IRequestHandler<RequestCheckCommand>
    {
        private readonly IDbContext dbContext;

        public RequestCheckCommandHandler(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(RequestCheckCommand request, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FindAsync(new object[] {request.Id}, cancellationToken);
            project.CheckRequested = true;
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}