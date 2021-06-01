namespace Isitar.DependencyUpdater.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Application.DependencyMgt.Commands.UpdateProjectDependency;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class DebugController : Controller
    {
        private readonly IMediator mediator;

        public DebugController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpGet("/debug")]
        public async Task<ActionResult> Debug()
        {
            await mediator.Send(new UpdateProjectDependencyCommand
            {
                ProjectId = Guid.Parse("92470A9E-00BD-4533-874C-0C306FD9F013"),
            });
            
            return Ok();
        }
    }
}