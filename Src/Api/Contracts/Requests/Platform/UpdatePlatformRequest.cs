namespace Isitar.DependencyUpdater.Api.Contracts.Requests.Platform
{
    using Application.Common.Mappings;
    using Application.Platform.Commands.UpdatePlatform;
    using AutoMapper;
    using Domain.Entities;

    public class UpdatePlatformRequest : IMapTo<UpdatePlatformCommand>
    {
        public string Name { get; set; }
        public PlatformType PlatformType { get; set; }
        public string PrivateKey { get; set; }
        public string ApiBaseUrl { get; set; }
        public string Token { get; set; }
        public string GitUserName { get; set; }
        public string GitUserEmail { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePlatformRequest, UpdatePlatformCommand>()
                .ForMember(cmd => cmd.Id, opts => opts.Ignore())
                ;
        }
    }
}