using FluentValidation;
using Identity.Api.Application.Commands;
using Identity.Domain.Aggregates.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Returns a list of users
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns list of users</response>
        /// <response code="401">If Unauthorized</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType<List<User>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<User>>> Get()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <returns>A newly created user</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/User
        ///     {
        ///        "name": "John Doe",
        ///        "email": "jdoe@gmail.com",
        ///        "password": "ThisIsAPassword"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">>Returns the newly created user</response>
        /// <response code="400">If the creation fails for some validation</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> Post(CreateUser user)
        {
            try
            {
                var createdUser = await _userService.CreateAsync(user.ToDomain());
                return CreatedAtAction(nameof(Post), new { id = createdUser.Id }, createdUser);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest("Email already registered, please login");
            }
        }
    }
}
