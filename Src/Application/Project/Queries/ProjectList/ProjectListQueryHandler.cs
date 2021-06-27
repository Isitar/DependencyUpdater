namespace Isitar.DependencyUpdater.Application.Project.Queries.ProjectList
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Services;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class ProjectListQueryHandler : IRequestHandler<ProjectListQuery, IEnumerable<ProjectListVm>>
    {
        private readonly IDbContext dbContext;
        private readonly IMapper mapper;

        public ProjectListQueryHandler(IDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProjectListVm>> Handle(ProjectListQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = dbContext.Projects.AsQueryable();
            if (request.IsOutdated.HasValue)
            {
                baseQuery = baseQuery.Where(p => p.IsOutdated == request.IsOutdated.Value);
            }

            return await baseQuery
                .ProjectTo<ProjectListVm>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}