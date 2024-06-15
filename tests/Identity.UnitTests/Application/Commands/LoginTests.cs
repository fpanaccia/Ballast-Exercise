using Identity.Api.Application.Commands;

namespace Identity.UnitTests.Application.Commands
{
    public class LoginTests
    {
        [Fact]
        public void Login_ShouldSetPropertiesCorrectly()
        {
            var email = "jdoe@gmail.com";
            var password = "password";

            var login = new Login
            {
                Email = email,
                Password = password
            };

            Assert.Equal(email, login.Email);
            Assert.Equal(password, login.Password);
        }
    }
}
