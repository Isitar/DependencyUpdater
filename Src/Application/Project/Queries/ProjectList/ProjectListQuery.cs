namespace Isitar.DependencyUpdater.Application.Project.Queries.ProjectList
{
    using System.Collections.Generic;
    using MediatR;

    public class ProjectListQuery : IRequest<IEnumerable<ProjectListVm>>
    {
        public bool? IsOutdated { get; set; }
    }
}