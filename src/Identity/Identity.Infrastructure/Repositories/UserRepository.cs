using Npgsql;
using Identity.Domain.Aggregates.User;
using System.Data;

namespace Identity.Infrastructure.Repositories
{
    public class UserRepository(NpgsqlConnection connection) : IUserRepository, IDisposable
    {
        public async Task<User> AddAsync(User user)
        {
            string insertQuery =
                "INSERT INTO users(id, name, email, password) VALUES (@Id, @Name, @Email, @Password)";

            using var cmd = connection.CreateCommand();
            cmd.CommandText = insertQuery;
            AddParameters(cmd, user);

            await connection.OpenAsync();
            var rowAffected = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return user;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            User? user = null;
            using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT id, name, email, password FROM users WHERE email = @Email;";
            cmd.Parameters.AddWithValue("@Email", email);
            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();  

            if (reader is not null)
            {
                if (await reader.ReadAsync())
                {
                    user = MapToDomain(reader);
                }
            }

            await connection.CloseAsync();
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = new List<User>();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT id, name, email, password FROM users;";
            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (reader is not null)
            {
                while (await reader.ReadAsync())
                {
                    users.Add(MapToDomain(reader));
                }
            }
            await connection.CloseAsync();

            return users;
        }

        public void Dispose()
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
            GC.SuppressFinalize(this);
        }

        private static void AddParameters(NpgsqlCommand command, User user)
        {
            var parameters = command.Parameters;

            parameters.AddWithValue("@Id", user.Id);
            parameters.AddWithValue("@Name", user.Name);
            parameters.AddWithValue("@Email", user.Email);
            parameters.AddWithValue("@Password", user.Password);
        }

        private static User MapToDomain(NpgsqlDataReader reader)
        {
            return new User(
                id: Guid.Parse(Convert.ToString(reader["id"])),
                name: Convert.ToString(reader["name"]),
                email: Convert.ToString(reader["email"]),
                password: Convert.ToString(reader["password"])
            );
        }
    }
}
