namespace Isitar.DependencyUpdater.Application.Project.Commands.RequestCheck
{
    using System;
    using MediatR;

    public class RequestCheckCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}