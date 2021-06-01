namespace Isitar.DependencyUpdater.Api.Controllers.V1
{
    using System;
    using System.Threading.Tasks;
    using Application.Project.Commands.CreateProject;
    using Contracts.Requests.Project;
    using Contracts.V1;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class PlatformProjectController : ApiController
    {
        [HttpPost(Routes.Platform.CreateProject, Name = nameof(PlatformProjectController) + "/" + nameof(CreateProjectAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateProjectAsync(Guid id, CreateProjectRequest createProjectRequest)
        {
            var cmd = Mapper.Map<CreateProjectCommand>(createProjectRequest);
            cmd.Id = Guid.NewGuid();
            cmd.PlatformId = id;
            await Mediator.Send(cmd);
            return CreatedAtRoute(nameof(ProjectController) + "/" + nameof(ProjectController.ProjectDetailAsync), new {id = cmd.Id}, null);
        }
    }
}