namespace Isitar.DependencyUpdater.Application.Platform.Queries.PlatformDetail
{
    using System;
    using MediatR;

    public class PlatformDetailQuery : IRequest<PlatformDetailVm>
    {
        public Guid Id { get; set; }
    }
}