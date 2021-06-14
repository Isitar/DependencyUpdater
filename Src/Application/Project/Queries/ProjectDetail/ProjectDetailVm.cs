namespace Isitar.DependencyUpdater.Application.Project.Queries.ProjectDetail
{
    using System;
    using AutoMapper;
    using Common.Mappings;
    using Domain.Entities;

    public class ProjectDetailVm : IMapFrom<Project>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PlatformId { get; set; }
        public string PlatformName { get; set; }
        public ProjectType ProjectType { get; set; }
        public string UpdateFrequency { get; set; }
        public string TargetBranch { get; set; }
        public string PlatformProjectId { get; set; }
        public string Url { get; set; }

        public bool IsOutdated { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Project, ProjectDetailVm>()
                .ForMember(vm => vm.PlatformName, opts => opts.MapFrom(proj => proj.Platform.Name))
                ;
        }
    }
}