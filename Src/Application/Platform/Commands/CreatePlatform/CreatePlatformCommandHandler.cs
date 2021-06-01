namespace Isitar.DependencyUpdater.Application.Platform.Commands.CreatePlatform
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Common.Services;
    using Domain.Entities;
    using MediatR;

    public class CreatePlatformCommandHandler : IRequestHandler<CreatePlatformCommand>
    {
        private readonly IDbContext dbContext;
        private readonly IPublisher publisher;

        public CreatePlatformCommandHandler(IDbContext dbContext, IPublisher publisher)
        {
            this.dbContext = dbContext;
            this.publisher = publisher;
        }

        public async Task<Unit> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
        {
            await dbContext.Platforms.AddAsync(new Platform
            {
                Id = request.Id,
                Name = request.Name.TrimToNull(),
                PlatformType = request.PlatformType,
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}