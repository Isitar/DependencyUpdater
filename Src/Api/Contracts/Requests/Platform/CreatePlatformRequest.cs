namespace Isitar.DependencyUpdater.Api.Contracts.Requests.Platform
{
    using Application.Common.Mappings;
    using Application.Platform.Commands;
    using Application.Platform.Commands.CreatePlatform;
    using AutoMapper;
    using Domain.Entities;

    public class CreatePlatformRequest : IMapTo<CreatePlatformCommand>
    {
        public string Name { get; set; }
        public PlatformType PlatformType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePlatformRequest, CreatePlatformCommand>()
                .ForMember(cmd => cmd.Id, opts => opts.Ignore())
                ;
        }
    }
}