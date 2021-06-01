namespace Isitar.DependencyUpdater.Api.Controllers.V1
{
    using System;
    using System.Threading.Tasks;
    using Application.Platform.Commands.CreatePlatform;
    using Application.Platform.Commands.UpdatePlatform;
    using Application.Platform.Queries.PlatformDetail;
    using Contracts.Requests.Platform;
    using Contracts.V1;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class PlatformController : ApiController
    {
        [HttpPost(Routes.Platform.CreatePlatform, Name = nameof(PlatformController) + "/" + nameof(CreatePlatformAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePlatformAsync(CreatePlatformRequest createPlatformRequest)
        {
            var cmd = Mapper.Map<CreatePlatformCommand>(createPlatformRequest);
            cmd.Id = Guid.NewGuid();
            await Mediator.Send(cmd);
            return CreatedAtRoute(nameof(PlatformController) + "/" + nameof(PlatformDetailAsync), new {id = cmd.Id}, null);
        }

        [HttpPatch(Routes.Platform.UpdatePlatform, Name = nameof(PlatformController) + "/" + nameof(UpdatePlatformAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePlatformAsync(Guid id, UpdatePlatformRequest updatePlatformRequest)
        {
            var cmd = Mapper.Map<UpdatePlatformCommand>(updatePlatformRequest);
            cmd.Id = id;
            await Mediator.Send(cmd);
            return Ok();
        }

        [HttpGet(Routes.Platform.PlatformDetail, Name = nameof(PlatformController) + "/" + nameof(PlatformDetailAsync))]
        public async Task<IActionResult> PlatformDetailAsync(Guid id)
        {
            var result = await Mediator.Send(new PlatformDetailQuery {Id = id});
            return Ok(result);
        }
    }
}