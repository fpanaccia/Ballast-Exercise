using FluentValidation;
using Identity.Api.Application.Commands;
using Identity.Domain.Aggregates.User;
using Identity.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Creates a valid token
        /// </summary>
        /// <returns>A bearer token</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Auth/Token
        ///     {
        ///        "email": "jdoe@gmail.com",
        ///        "password": "ThisIsAPassword"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">>Returns the bearer token</response>
        /// <response code="400">If the password is invalid</response>
        /// <response code="404">If the user does not exists</response>
        [HttpPost("Token")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> Token(Login login)
        {
            try
            {
                var token = await _authService.LoginAttempt(login.Email, login.Password);
                return Ok(token);
            }
            catch (UserDoesNotExistException)
            {
                return NotFound("User does not exists");
            }
            catch (InvalidPasswordException)
            {
                return BadRequest("Invalid Password");
            }
        }
    }
}
