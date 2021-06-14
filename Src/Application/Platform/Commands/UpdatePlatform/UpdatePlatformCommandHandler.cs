namespace Isitar.DependencyUpdater.Application.Platform.Commands.UpdatePlatform
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Common.Services;
    using MediatR;

    public class UpdatePlatformCommandHandler : IRequestHandler<UpdatePlatformCommand>
    {
        private readonly IDbContext dbContext;

        public UpdatePlatformCommandHandler(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdatePlatformCommand request, CancellationToken cancellationToken)
        {
            var platform = await dbContext.Platforms.FindAsync(new object[] {request.Id}, cancellationToken);
            platform.Name = request.Name.TrimToNull();
            platform.PlatformType = request.PlatformType;
            platform.PrivateKey = request.PrivateKey.TrimToNull() + Environment.NewLine;
            platform.ApiBaseUrl = request.ApiBaseUrl.TrimToNull();
            platform.Token = request.Token.TrimToNull();
            platform.GitUserName = request.GitUserName.TrimToNull();
            platform.GitUserEmail = request.GitUserEmail.TrimToNull();

            await dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}