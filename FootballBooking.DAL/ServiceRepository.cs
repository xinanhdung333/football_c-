using FootballBooking.Models;
using Microsoft.Data.SqlClient;

namespace FootballBooking.DAL;

public class ServiceRepository
{
    public async Task<IEnumerable<Service>> GetAllServicesAsync()
    {
        const string sql = @"SELECT id, name, description, price, status, image, quantity, created_at, updated_at FROM services";

        var services = new List<Service>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            services.Add(new Service
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                Price = reader.GetDecimal(3),
                Status = Enum.Parse<ServiceStatus>(reader.GetString(4), true),
                Image = reader.IsDBNull(5) ? null : reader.GetString(5),
                Quantity = reader.GetInt32(6),
                CreatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
            });
        }

        return services;
    }

    public async Task<Service?> GetServiceByIdAsync(int id)
    {
        const string sql = @"SELECT id, name, description, price, status, image, quantity, created_at, updated_at FROM services WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new Service
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
            Price = reader.GetDecimal(3),
            Status = Enum.Parse<ServiceStatus>(reader.GetString(4), true),
            Image = reader.IsDBNull(5) ? null : reader.GetString(5),
            Quantity = reader.GetInt32(6),
            CreatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
            UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
        };
    }

    public async Task<int> CreateServiceAsync(Service service)
    {
        const string sql = @"
INSERT INTO services (name, description, price, status, image, quantity)
OUTPUT INSERTED.id
VALUES (@name, @description, @price, @status, @image, @quantity)";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@name", service.Name);
        command.Parameters.AddWithValue("@description", service.Description ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@price", service.Price);
        command.Parameters.AddWithValue("@status", service.Status.ToString().ToLowerInvariant());
        command.Parameters.AddWithValue("@image", service.Image ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@quantity", service.Quantity);

        await connection.OpenAsync();
        var insertedId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(insertedId);
    }

    public async Task UpdateServiceAsync(Service service)
    {
        const string sql = @"
UPDATE services
SET name = @name,
    description = @description,
    price = @price,
    status = @status,
    image = @image,
    quantity = @quantity,
    updated_at = GETDATE()
WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", service.Id);
        command.Parameters.AddWithValue("@name", service.Name);
        command.Parameters.AddWithValue("@description", service.Description ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@price", service.Price);
        command.Parameters.AddWithValue("@status", service.Status.ToString().ToLowerInvariant());
        command.Parameters.AddWithValue("@image", service.Image ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@quantity", service.Quantity);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteServiceAsync(int id)
    {
        const string sql = @"DELETE FROM services WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateQuantityAsync(int id, int quantity)
    {
        await using var connection = SqlConnectionFactory.CreateConnection();
        await connection.OpenAsync();
        await UpdateQuantityAsync(id, quantity, connection, null);
    }

    public async Task UpdateQuantityAsync(int id, int quantity, SqlConnection connection, SqlTransaction? transaction)
    {
        const string sql = @"UPDATE services SET quantity = @quantity, updated_at = GETDATE() WHERE id = @id";

        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@quantity", quantity);

        await command.ExecuteNonQueryAsync();
    }
}
