using Npgsql;
using Aircraft.Domain.Aggregates.Airplane;
using System.Data;

namespace Aircraft.Infrastructure.Repositories
{
    public class AirplaneRepository(NpgsqlConnection connection) : IAirplaneRepository, IDisposable
    {
        public async Task<Airplane> AddAsync(Airplane airplane)
        {
            string insertQuery =
                "INSERT INTO airplanes(id, model, weight, manufacturer) VALUES (@Id, @Model, @Weight, @Manufacturer)";

            using var cmd = connection.CreateCommand();
            cmd.CommandText = insertQuery;
            AddParameters(cmd, airplane);

            await connection.OpenAsync();
            var rowAffected = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return airplane;
        }

        public async Task<Airplane?> FindByIdAsync(Guid id)
        {
            Airplane? airplane = null;
            using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT id, model, weight, manufacturer FROM airplanes WHERE id = @ID;";
            cmd.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();  

            if (reader is not null)
            {
                if (await reader.ReadAsync())
                {
                    airplane = MapToDomain(reader);
                }
            }

            await connection.CloseAsync();
            return airplane;
        }

        public async Task DeleteAsync(Airplane airplane)
        {
            string deleteQuery = "DELETE FROM airplanes WHERE id = @Id";
            using var cmd = connection.CreateCommand();

            cmd.CommandText = deleteQuery;
            AddParameters(cmd, airplane);
            await connection.OpenAsync();
            var rowAffected = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

        public async Task<List<Airplane>> GetAllAsync()
        {
            var airplanes = new List<Airplane>();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT id, model, weight, manufacturer FROM airplanes;";
            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (reader is not null)
            {
                while (await reader.ReadAsync())
                {
                    airplanes.Add(MapToDomain(reader));
                }
            }
            await connection.CloseAsync();

            return airplanes;
        }

        public async Task<Airplane> UpdateAsync(Airplane airplane)
        {
            string updateQuery =
                "UPDATE airplanes SET id=@Id, model=@Model, weight=@Weight, manufacturer=@Manufacturer WHERE id = @Id";

            using var cmd = connection.CreateCommand();
            cmd.CommandText = updateQuery;
            AddParameters(cmd, airplane);
            await connection.OpenAsync();
            var rowAffected = await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            return airplane;
        }

        public void Dispose()
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
            GC.SuppressFinalize(this);
        }

        private static void AddParameters(NpgsqlCommand command, Airplane airplane)
        {
            var parameters = command.Parameters;

            parameters.AddWithValue("@Id", airplane.Id);
            parameters.AddWithValue("@Model", airplane.Model);
            parameters.AddWithValue("@Weight", airplane.Weight);
            parameters.AddWithValue("@Manufacturer", airplane.Manufacturer);
        }

        private static Airplane MapToDomain(NpgsqlDataReader reader)
        {
            return new Airplane(
                id: Guid.Parse(Convert.ToString(reader["id"])),
                model: Convert.ToString(reader["model"]),
                weight: Convert.ToString(reader["weight"]),
                manufacturer: Convert.ToString(reader["manufacturer"])
            );
        }
    }
}
