using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Splitit.Models;
using Splitit.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Splitit.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActorsController : ControllerBase
    {
        public ActorsController(IActorService service)
        {
            _actorService = service;
        }

        private IActorService _actorService;


        [HttpGet]
        [SwaggerResponse(200, "Leading Actors", typeof(ActorDTO))]
        [SwaggerResponse(400, "Bad Request")]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] string name = null, [FromQuery] int? minRank = null, [FromQuery] int? maxRank = null, [FromQuery] int skip = 0,
        [FromQuery] int take = 20)
        {
            try
            {
                var actors = await _actorService.Get(name, minRank, maxRank);
                List<ActorDTO> res = new List<ActorDTO>();
                if (skip < actors.Count)
                {
                    res = actors.Skip(skip).Take(take).ToList().Select(x => ActorDTO.ActorToDTO(x)).ToList();
                }
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("{id}")]
        [SwaggerResponse(200, "Single Actor Details", typeof(Actor))]
        [SwaggerResponse(400, "Bad Request/ Actor not found")]
        public async Task<ActionResult<List<Actor>>> Get(int id)
        {
            try
            {
                var actor = await _actorService.Get(id);
                if (actor != null)
                {
                    return Ok(actor);
                }
                return BadRequest("Actor not found");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [SwaggerResponse(204, "Actor updated")]
        [SwaggerResponse(400, "Actor not found/ Rank already exists in another actor/ general error")]
        public async Task<IActionResult> Update([FromBody]Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            if (actor == null)
            {
                return BadRequest("Actor is null");
            }

            if (!await _actorService.IsRankUnique(actor))
            {
                return BadRequest("Rank must be unique");
            }

            try
            {
                if (await _actorService.Update(actor))
                    return NoContent();
                else
                {
                    return BadRequest("Actor not found");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete]
        [SwaggerResponse(204, "Actor Deleted")]
        [SwaggerResponse(400, "Actor not found/ general error")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await _actorService.Remove(id))
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("Actor not found");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [SwaggerResponse(204, "Actor Added")]
        [SwaggerResponse(400, "Actor not found/ Rank already exists in another actor/ General error")]
        public async Task<IActionResult> Add([FromBody]Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (actor == null)
            {
                return BadRequest("Actor is null");
            }

            if (!await _actorService.IsRankUnique(actor))
            {
                return BadRequest("Rank must be unique");
            }

            try
            {
                await _actorService.Add(actor);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
