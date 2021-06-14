namespace Isitar.DependencyUpdater.Application.Project.Queries.ProjectList
{
    using System;
    using AutoMapper;
    using Common.Mappings;
    using Domain.Entities;

    public class ProjectListVm : IMapFrom<Project>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }

        public bool IsOutdated { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Project, ProjectListVm>()
                .ForMember(vm => vm.Platform, opts => opts.MapFrom(dbo => dbo.Platform.Name))
                ;
        }
    }
}