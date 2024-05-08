using ActorManagement.Models;
using ActorManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ActorManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;
        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public async Task<ActionResult<ActorsResponses>> Actors(string nameDescription, int minRank = 0, int maxRank, int page = 1, int pageSize = 10)
         {
            ActorsResponses actorRespone = new ActorsResponses();
            actorRespone.Actors = _actorService.GetAllActorsAsync(nameDescription, minRank, maxRank , page , pageSize).Result;
            return Ok(actorRespone);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Actor(string id)
        {
            var actor = await _actorService.GetActorByIdAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return Ok(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Actor([FromBody] Actor actor)
        {
            if (actor == null)
            {
                return BadRequest();
            }

            var createdActor = await _actorService.AddActorAsync(actor);
            return CreatedAtRoute("Actor", new { id = createdActor.Id }, createdActor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(string id)
        {
            ActorResponse response = _actorService.DeleteActorAsync(id);
            if (!response.IsSuccess)
            {
                response.TraceId = id;
            }
            return Ok(response);
        }
    }
}
