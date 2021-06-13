namespace Isitar.DependencyUpdater.Domain.Entities
{
    using System;

    public enum PlatformType
    {
        GitLab = 1,
        GitHub = 2,
    }

    public class Platform
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PlatformType PlatformType { get; set; }
        public string PrivateKey { get; set; }
        public string ApiBaseUrl { get; set; }
        public string Token { get; set; }
    }
}