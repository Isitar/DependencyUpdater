namespace Isitar.DependencyUpdater.Application.Platform.Queries.PlatformList
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Services;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class PlatformListQueryHandler : IRequestHandler<PlatformListQuery, IEnumerable<PlatformListVm>>
    {
        private readonly IDbContext dbContext;
        private readonly IMapper mapper;

        public PlatformListQueryHandler(IDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PlatformListVm>> Handle(PlatformListQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Platforms
                .ProjectTo<PlatformListVm>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}