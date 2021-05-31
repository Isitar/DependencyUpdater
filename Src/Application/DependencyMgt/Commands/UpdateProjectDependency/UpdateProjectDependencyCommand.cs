namespace Isitar.DependencyUpdater.Application.DependencyMgt.Commands.UpdateProjectDependency
{
    using System;
    using MediatR;

    public class UpdateProjectDependencyCommand : IRequest
    {
        public Guid ProjectId { get; set; }
    }
}