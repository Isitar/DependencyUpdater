namespace Isitar.DependencyUpdater.Application.Platform.Queries.PlatformDetail
{
    using System;
    using Common.Mappings;
    using Domain.Entities;

    public class PlatformDetailVm : IMapFrom<Platform>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PlatformType PlatformType { get; set; }
        public string PrivateKey { get; set; }
        public string ApiBaseUrl { get; set; }
        public string Token { get; set; }
    }
}