namespace Identity.UnitTests.Domain.Aggregates.User
{
    public class UserTests
    {
        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            var id = Guid.NewGuid();
            var name = "John Doe";
            var email = "jdoe@gmail.com";
            var password = "password";

            var user = new Identity.Domain.Aggregates.User.User(id, name, email, password);

            Assert.Equal(id, user.Id);
            Assert.Equal(name, user.Name);
            Assert.Equal(email, user.Email);
            Assert.Equal(password, user.Password);
        }
    }
}
