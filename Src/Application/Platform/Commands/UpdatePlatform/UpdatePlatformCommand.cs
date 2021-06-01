namespace Isitar.DependencyUpdater.Application.Platform.Commands.UpdatePlatform
{
    using System;
    using Domain.Entities;
    using MediatR;

    public class UpdatePlatformCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public PlatformType PlatformType { get; set; }
        public string PrivateKey { get; set; }
        public string ApiBaseUrl { get; set; }
        public string Token { get; set; }
    }
}