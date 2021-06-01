namespace Isitar.DependencyUpdater.Application.Platform.Commands.CreatePlatform
{
    using System;
    using Domain.Entities;
    using MediatR;

    public class CreatePlatformCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PlatformType PlatformType { get; set; }
    }
}