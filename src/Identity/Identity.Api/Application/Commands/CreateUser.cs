using Identity.Domain.Aggregates.User;

namespace Identity.Api.Application.Commands
{
    public class CreateUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User ToDomain()
        {
            return new User(id: Guid.NewGuid(), name: Name, email: Email, password: Password);
        }

    }
}
