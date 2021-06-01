namespace Isitar.DependencyUpdater.Application.Project.Commands.CreateProject
{
    using System;
    using Domain.Entities;
    using MediatR;

    public class CreateProjectCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PlatformId { get; set; }
        public ProjectType ProjectType { get; set; }
    }
}