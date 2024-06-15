using FluentValidation;
using Aircraft.Domain.Aggregates.Airplane;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using Aircraft.Api.Application.Commands;

namespace Aircraft.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class AirplaneController : ControllerBase
    {
        private readonly IAirplaneService _airplaneService;

        public AirplaneController(IAirplaneService airplaneService)
        {
            _airplaneService = airplaneService;
        }

        /// <summary>
        /// Returns a list of airplanes
        /// </summary>
        /// <returns>List of airplanes</returns>
        /// <response code="200">Returns list of airplanes</response>
        /// <response code="401">If Unauthorized</response>
        [HttpGet]
        [ProducesResponseType<List<Airplane>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<Airplane>>> Get()
        {
            var users = await _airplaneService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Returns an airplane
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An airplane</returns>
        /// <response code="200">Returns an airplane</response>
        /// <response code="401">If Unauthorized</response>
        /// <response code="404">If the airplane does not exists</response>
        [HttpGet("{id}")]
        [ProducesResponseType<Airplane>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Airplane>> Get(Guid id)
        {
            var user = await _airplaneService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Creates an aircraft
        /// </summary>
        /// <returns>An aircraft</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Airplane
        ///     {
        ///        "model": "B737-800",
        ///        "weight": "41140 Kg",
        ///        "manufacturer": "Boeing"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">>Returns the aircraft created</response>
        /// <response code="400">If the aircraft is invalid</response>
        /// <response code="401">If Unauthorized</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Airplane>> Post(CreateAirplane airplane)
        {
            try
            {
                var createdAirplane = await _airplaneService.CreateAsync(airplane.ToDomain());
                return CreatedAtAction(nameof(Post), new { id = createdAirplane.Id }, createdAirplane);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        /// <summary>
        /// Updates an aircraft
        /// </summary>
        /// <returns>An aircraft</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Airplane
        ///     {
        ///        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "model": "B737-800",
        ///        "weight": "41140 Kg",
        ///        "manufacturer": "Boeing"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">>Returns the aircraft updated</response>
        /// <response code="400">If the aircraft is invalid</response>
        /// <response code="401">If Unauthorized</response>
        /// <response code="404">If the aircraft does not exists</response>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<Airplane>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Airplane>> Put(Guid id, UpdateAirplane airplane)
        {
            if (id != airplane.Id)
            {
                return BadRequest("ID does not correspond with id in object");
            }

            try
            {
                var updatedAirplane = await _airplaneService.UpdateAsync(airplane.ToDomain());
                if (updatedAirplane == null)
                {
                    return NotFound();
                }

                return Ok(updatedAirplane);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        /// <summary>
        /// Deletes a specific airplane.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">>If aircraft is deleted</response>
        /// <response code="401">If Unauthorized</response>
        /// <response code="404">If the aircraft does not exists</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _airplaneService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _airplaneService.DeletAsync(id);
            return Ok();
        }
    }
}
