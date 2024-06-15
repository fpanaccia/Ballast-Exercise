using Identity.Api.Application.Commands;

namespace Identity.UnitTests.Application.Commands
{
    public class CreateUserTests
    {
        [Fact]
        public void ToDomain_ShouldReturnUser_WithCorrectProperties()
        {
            var createUser = new CreateUser
            {
                Name = "John Doe",
                Email = "jdoe@gmail.com",
                Password = "password"
            };

            var user = createUser.ToDomain();

            Assert.NotNull(user);
            Assert.Equal(createUser.Name, user.Name);
            Assert.Equal(createUser.Email, user.Email);
            Assert.Equal(createUser.Password, user.Password);
            Assert.NotEqual(Guid.Empty, user.Id);
        }

        [Fact]
        public void ToDomain_ShouldGenerateNewGuid()
        {
            var createUser = new CreateUser
            {
                Name = "Jane Doe",
                Email = "jane@gmail.com",
                Password = "password"
            };

            var user1 = createUser.ToDomain();
            var user2 = createUser.ToDomain();

            Assert.NotEqual(user1.Id, user2.Id);
        }
    }
}
