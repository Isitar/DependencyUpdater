namespace Isitar.DependencyUpdater.Api.Contracts.Requests.Project
{
    using Application.Common.Mappings;
    using Application.Project.Commands.CreateProject;
    using AutoMapper;
    using Domain.Entities;

    public class CreateProjectRequest : IMapTo<CreateProjectCommand>
    {
        public string Name { get; set; }
        public ProjectType ProjectType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateProjectRequest, CreateProjectCommand>()
                .ForMember(cmd => cmd.Id, opts => opts.Ignore())
                .ForMember(cmd => cmd.PlatformId, opts => opts.Ignore())
                ;
        }
    }
}