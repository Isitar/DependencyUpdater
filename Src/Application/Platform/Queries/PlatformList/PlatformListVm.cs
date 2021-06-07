namespace Isitar.DependencyUpdater.Application.Platform.Queries.PlatformList
{
    using System;
    using Common.Mappings;
    using Domain.Entities;

    public class PlatformListVm : IMapFrom<Platform>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PlatformType PlatformType { get; set; }
    }
}