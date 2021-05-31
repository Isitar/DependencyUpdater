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
                ProjectId = Guid.Parse("F9664465-29EE-475C-B7AA-55E22D141BAE"),
            });
            
            return Ok();
        }
    }
}