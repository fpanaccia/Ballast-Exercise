using System.IdentityModel.Tokens.Jwt;
using Identity.Api.Application.Services;
using Identity.Domain.Aggregates.User;
using Identity.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Identity.UnitTests.Application.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockUserRepository = new Mock<IUserRepository>();
            _authService = new AuthService(_mockConfiguration.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task LoginAttempt_ShouldThrowUserDoesNotExistException_WhenUserDoesNotExist()
        {
            var email = "jdoe@gmail.com";
            var password = "password";

            _mockUserRepository.Setup(r => r.FindByEmailAsync(email)).ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<UserDoesNotExistException>(() => _authService.LoginAttempt(email, password));
        }

        [Fact]
        public async Task LoginAttempt_ShouldThrowInvalidPasswordException_WhenPasswordIsIncorrect()
        {
            var email = "jdoe@gmail.com";
            var password = "password";
            var user = new User(Guid.NewGuid(), "John Doe", email, "wrongpassword");

            _mockUserRepository.Setup(r => r.FindByEmailAsync(email)).ReturnsAsync(user);

            await Assert.ThrowsAsync<InvalidPasswordException>(() => _authService.LoginAttempt(email, password));
        }

        [Fact]
        public async Task LoginAttempt_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var email = "jdoe@gmail.com";
            var password = "password";
            var user = new User(Guid.NewGuid(), "John Doe", email, password);

            _mockUserRepository.Setup(r => r.FindByEmailAsync(email)).ReturnsAsync(user);
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("ThisIsJustAKeyNeededForTheTokenGeneration");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("ThisIsAValidIssuer");

            var token = await _authService.LoginAttempt(email, password);

            Assert.NotNull(token);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            Assert.Equal(email, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value);
        }
    }
}
