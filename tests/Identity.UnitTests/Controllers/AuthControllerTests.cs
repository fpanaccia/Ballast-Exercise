using Identity.Api.Application.Commands;
using Identity.Api.Controllers;
using Identity.Domain.Aggregates.User;
using Identity.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Identity.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _authController = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Token_ShouldReturnOk_WithToken_WhenCredentialsAreValid()
        {
            var login = new Login { Email = "jdoe@gmail.com", Password = "password" };
            var token = "validToken";

            _mockAuthService.Setup(s => s.LoginAttempt(login.Email, login.Password)).ReturnsAsync(token);

            var result = await _authController.Token(login);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(token, okResult.Value);
        }

        [Fact]
        public async Task Token_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            var login = new Login { Email = "jdoe@gmail.com", Password = "password" };

            _mockAuthService.Setup(s => s.LoginAttempt(login.Email, login.Password)).ThrowsAsync(new UserDoesNotExistException());

            var result = await _authController.Token(login);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("User does not exists", notFoundResult.Value);
        }

        [Fact]
        public async Task Token_ShouldReturnBadRequest_WhenPasswordIsInvalid()
        {
            var login = new Login { Email = "jdoe@gmail.com", Password = "wrongpassword" };

            _mockAuthService.Setup(s => s.LoginAttempt(login.Email, login.Password)).ThrowsAsync(new InvalidPasswordException());

            var result = await _authController.Token(login);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Invalid Password", badRequestResult.Value);
        }
    }
}
