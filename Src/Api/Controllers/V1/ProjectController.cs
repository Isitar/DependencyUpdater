namespace Isitar.DependencyUpdater.Api.Controllers.V1
{
    using System;
    using System.Threading.Tasks;
    using Application.DependencyMgt.Commands.UpdateProjectDependency;
    using Application.Project.Commands.UpdateProject;
    using Application.Project.Queries.ProjectDetail;
    using Application.Project.Queries.ProjectList;
    using Contracts.Requests.Project;
    using Contracts.V1;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class ProjectController : ApiController
    {
        [HttpGet(Routes.Project.ProjectDetail, Name = nameof(ProjectController) + "/" + nameof(ProjectDetailAsync))]
        public async Task<IActionResult> ProjectDetailAsync(Guid id)
        {
            var result = await Mediator.Send(new ProjectDetailQuery {Id = id});
            return Ok(result);
        }

        [HttpPatch(Routes.Project.UpdateProject, Name = nameof(ProjectController) + "/" + nameof(UpdateProjectAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProjectAsync(Guid id, UpdateProjectRequest updateProjectRequest)
        {
            var cmd = Mapper.Map<UpdateProjectCommand>(updateProjectRequest);
            cmd.Id = id;
            await Mediator.Send(cmd);
            return Ok();
        }

        [HttpGet(Routes.Project.AllProjects, Name = nameof(ProjectController) + "/" + nameof(AllProjectsAsync))]
        public async Task<IActionResult> AllProjectsAsync()
        {
            var result = await Mediator.Send(new ProjectListQuery());
            return Ok(result);
        }

        [HttpPost(Routes.Project.UpdateDependencies, Name = nameof(ProjectController) + "/" + nameof(UpdateDependenciesAsync))]
        public async Task<IActionResult> UpdateDependenciesAsync(Guid id)
        {
            await Mediator.Send(new UpdateProjectDependencyCommand {ProjectId = id});
            return Ok();
        }
    }
}