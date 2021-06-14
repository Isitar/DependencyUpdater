namespace Isitar.DependencyUpdater.Domain.Entities
{
    using System;
    using System.Security.Principal;

    [Flags]
    public enum ProjectType
    {
        Undefined,
        Dotnet,
        Npm,
    }
    
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; }
        public ProjectType ProjectType { get; set; }
        public string UpdateFrequency { get; set; }
        public string TargetBranch { get; set; }
        public string PlatformProjectId { get; set; }
        public string Url { get; set; }

        public bool IsOutdated { get; set; }
    }
}