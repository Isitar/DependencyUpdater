namespace Isitar.DependencyUpdater.Application.Project.Queries.ProjectDetail
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Services;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class ProjectDetailQueryHandler : IRequestHandler<ProjectDetailQuery, ProjectDetailVm>
    {
        private readonly IDbContext dbContext;
        private readonly IMapper mapper;

        public ProjectDetailQueryHandler(IDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ProjectDetailVm> Handle(ProjectDetailQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Projects
                .Where(p => p.Id.Equals(request.Id))
                .ProjectTo<ProjectDetailVm>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}