namespace Isitar.DependencyUpdater.Api.Contracts.Requests.Project
{
    using Application.Common.Mappings;
    using Application.Project.Commands.UpdateProject;
    using AutoMapper;
    using Domain.Entities;

    public class UpdateProjectRequest : IMapTo<UpdateProjectCommand>
    {
        public string Name { get; set; }
        public ProjectType ProjectType { get; set; }
        public string UpdateFrequency { get; set; }
        public string TargetBranch { get; set; }
        public string PlatformProjectId { get; set; }
        public string Url { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateProjectRequest, UpdateProjectCommand>()
                .ForMember(cmd => cmd.Id, opts => opts.Ignore())
                ;
        }
    }
}