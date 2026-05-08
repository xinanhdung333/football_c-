using FootballBooking.Models;
using Microsoft.Data.SqlClient;

namespace FootballBooking.DAL;

public class UserRepository
{
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        const string sql = @"SELECT id, name, avt, email, phone, password, role, created_at, updated_at FROM users WHERE email = @email";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@email", email);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new User
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Avt = reader.IsDBNull(2) ? null : reader.GetString(2),
            Email = reader.GetString(3),
            Phone = reader.GetString(4),
            Password = reader.IsDBNull(5) ? null : reader.GetString(5),
            Role = Enum.Parse<UserRole>(reader.GetString(6), true),
            CreatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
            UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
        };
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        const string sql = @"SELECT id, name, avt, email, phone, password, role, created_at, updated_at FROM users WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new User
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Avt = reader.IsDBNull(2) ? null : reader.GetString(2),
            Email = reader.GetString(3),
            Phone = reader.GetString(4),
            Password = reader.IsDBNull(5) ? null : reader.GetString(5),
            Role = Enum.Parse<UserRole>(reader.GetString(6), true),
            CreatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
            UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
        };
    }

    public async Task<int> CreateUserAsync(User user)
    {
        const string sql = @"
INSERT INTO users (name, avt, email, phone, password, role)
OUTPUT INSERTED.id
VALUES (@name, @avt, @email, @phone, @password, @role)";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@avt", user.Avt ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@phone", user.Phone);
        command.Parameters.AddWithValue("@password", user.Password ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@role", user.Role.ToString().ToLowerInvariant());

        await connection.OpenAsync();
        var insertedId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(insertedId);
    }

    public async Task UpdateUserAsync(User user)
    {
        const string sql = @"
UPDATE users
SET name = @name,
    avt = @avt,
    phone = @phone,
    updated_at = GETDATE()
WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", user.Id);
        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@avt", user.Avt ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@phone", user.Phone);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateUserRoleAsync(int id, UserRole role)
    {
        const string sql = @"
UPDATE users
SET role = @role,
    updated_at = GETDATE()
WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@role", role.ToString().ToLowerInvariant());

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateUserPasswordAsync(int id, string password)
    {
        const string sql = @"
UPDATE users
SET password = @password,
    updated_at = GETDATE()
WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@password", password);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        const string sql = @"DELETE FROM users WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        const string sql = @"SELECT id, name, avt, email, phone, password, role, created_at, updated_at FROM users ORDER BY created_at DESC";

        var users = new List<User>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Avt = reader.IsDBNull(2) ? null : reader.GetString(2),
                Email = reader.GetString(3),
                Phone = reader.GetString(4),
                Password = reader.IsDBNull(5) ? null : reader.GetString(5),
                Role = Enum.Parse<UserRole>(reader.GetString(6), true),
                CreatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
            });
        }

        return users;
    }
}