using FluentValidation;
using FluentValidation.Results;
using Identity.Api.Application.Commands;
using Identity.Api.Controllers;
using Identity.Domain.Aggregates.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Identity.UnitTests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _userController = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnOk_WithListOfUsers()
        {
            var users = new List<User>
            {
                new User(Guid.NewGuid(), "John Doe", "jdoe@gmail.com", "password"),
                new User(Guid.NewGuid(), "Jane Doe", "jane@gmail.com", "password")
            };

            _mockUserService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

            var result = await _userController.Get();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(users, okResult.Value);
        }

        [Fact]
        public async Task Post_ShouldReturnCreated_WhenUserIsValid()
        {
            var createUser = new CreateUser { Name = "John Doe", Email = "jdoe@gmail.com", Password = "password" };
            var user = createUser.ToDomain();

            _mockUserService.Setup(s => s.CreateAsync(It.IsAny<User>())).ReturnsAsync(user);
            var result = await _userController.Post(createUser);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(user, createdResult.Value);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenValidationFails()
        {
            var createUser = new CreateUser { Name = "John Doe", Email = "invalidemail", Password = "password" };
            var validationFailure = new ValidationFailure("Email", "Invalid email format");
            var validationResult = new ValidationResult(new List<ValidationFailure> { validationFailure });

            _mockUserService.Setup(s => s.CreateAsync(It.IsAny<User>())).ThrowsAsync(new ValidationException(validationResult.Errors));
            var result = await _userController.Post(createUser);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal(validationResult.Errors, badRequestResult.Value);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenEmailAlreadyExists()
        {
            var createUser = new CreateUser { Name = "John Doe", Email = "jdoe@gmail.com", Password = "password" };

            _mockUserService.Setup(s => s.CreateAsync(It.IsAny<User>())).ThrowsAsync(new Exception("Email already registered, please login"));

            var result = await _userController.Post(createUser);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Email already registered, please login", badRequestResult.Value);
        }
    }
}
