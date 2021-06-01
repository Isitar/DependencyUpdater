namespace Isitar.DependencyUpdater.Application.Project.Commands.UpdateProject
{
    using System;
    using Domain.Entities;
    using MediatR;

    public class UpdateProjectCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ProjectType ProjectType { get; set; }
        public string UpdateFrequency { get; set; }
        public string TargetBranch { get; set; }
        public string PlatformProjectId { get; set; }
        public string Url { get; set; }
    }
}