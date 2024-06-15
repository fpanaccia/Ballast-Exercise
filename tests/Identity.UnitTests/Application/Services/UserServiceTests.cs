using Moq;
using FluentValidation;
using FluentValidation.Results;
using Identity.Domain.Aggregates.User;
using Identity.Domain.Exceptions;
using Identity.Api.Application.Services;

namespace Identity.UnitTests.Application.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IValidator<User>> _mockValidator;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockValidator = new Mock<IValidator<User>>();
            _userService = new UserService(_mockUserRepository.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowValidationException_WhenUserIsInvalid()
        {
            var user = new User(Guid.NewGuid(), "", "jdoe@gmail.com", "password");
            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Name", "Name is required") });

            _mockValidator.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(validationResult);

            await Assert.ThrowsAsync<ValidationException>(() => _userService.CreateAsync(user));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowUserAlreadyExistException_WhenUserAlreadyExists()
        {
            var user = new User(Guid.NewGuid(), "John", "jdoe@gmail.com", "password");
            var validationResult = new ValidationResult();

            _mockValidator.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(validationResult);
            _mockUserRepository.Setup(r => r.FindByEmailAsync(user.Email)).ReturnsAsync(user);

            await Assert.ThrowsAsync<UserAlreadyExistException>(() => _userService.CreateAsync(user));
        }

        [Fact]
        public async Task CreateAsync_ShouldAddUser_WhenUserIsValidAndDoesNotExist()
        {
            var user = new User(Guid.NewGuid(), "John", "jdoe@gmail.com", "password");
            var validationResult = new ValidationResult();

            _mockValidator.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(validationResult);
            _mockUserRepository.Setup(r => r.FindByEmailAsync(user.Email)).ReturnsAsync((User?)null);
            _mockUserRepository.Setup(r => r.AddAsync(user)).ReturnsAsync(user);

            var result = await _userService.CreateAsync(user);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            _mockUserRepository.Verify(r => r.AddAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            var users = new List<User>
            {
                new User(Guid.NewGuid(), "John", "jdoe@gmail.com", "pass"),
                new User(Guid.NewGuid(), "Jane", "jane@gmail.com", "pass123")
            };

            _mockUserRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var result = await _userService.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count);
        }
    }
}
