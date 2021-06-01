namespace Isitar.DependencyUpdater.Api.Controllers.V1
{
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected IMediator Mediator => HttpContext.RequestServices.GetService<IMediator>();
        protected IMapper Mapper => HttpContext.RequestServices.GetService<IMapper>();
    }
}