namespace Isitar.DependencyUpdater.Application.Platform.Queries.PlatformDetail
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Services;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class PlatformDetailQueryHandler : IRequestHandler<PlatformDetailQuery, PlatformDetailVm>
    {
        private readonly IDbContext dbContext;
        private readonly IMapper mapper;

        public PlatformDetailQueryHandler(IDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<PlatformDetailVm> Handle(PlatformDetailQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Platforms
                .Where(p => p.Id.Equals(request.Id))
                .ProjectTo<PlatformDetailVm>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}