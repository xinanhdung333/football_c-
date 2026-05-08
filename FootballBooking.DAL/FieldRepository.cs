using FootballBooking.Models;
using Microsoft.Data.SqlClient;

namespace FootballBooking.DAL;

public class FieldRepository
{
    public async Task<IEnumerable<Field>> GetAllFieldsAsync()
    {
        const string sql = @"
SELECT id, name, location, description, price_per_hour, peak_start_time, peak_end_time, peak_price_per_hour, image, status, created_at, updated_at
FROM fields";

        var fields = new List<Field>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            fields.Add(new Field
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Location = reader.GetString(2),
                Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                PricePerHour = reader.GetDecimal(4),
                PeakStartTime = reader.IsDBNull(5) ? null : reader.GetTimeSpan(5),
                PeakEndTime = reader.IsDBNull(6) ? null : reader.GetTimeSpan(6),
                PeakPricePerHour = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                Image = reader.IsDBNull(8) ? null : reader.GetString(8),
                Status = Enum.Parse<FieldStatus>(reader.GetString(9), true),
                CreatedAt = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                UpdatedAt = reader.IsDBNull(11) ? null : reader.GetDateTime(11)
            });
        }

        return fields;
    }

    public async Task<Field?> GetFieldByIdAsync(int id)
    {
        const string sql = @"
SELECT id, name, location, description, price_per_hour, peak_start_time, peak_end_time, peak_price_per_hour, image, status, created_at, updated_at
FROM fields
WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new Field
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Location = reader.GetString(2),
            Description = reader.IsDBNull(3) ? null : reader.GetString(3),
            PricePerHour = reader.GetDecimal(4),
            PeakStartTime = reader.IsDBNull(5) ? null : reader.GetTimeSpan(5),
            PeakEndTime = reader.IsDBNull(6) ? null : reader.GetTimeSpan(6),
            PeakPricePerHour = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
            Image = reader.IsDBNull(8) ? null : reader.GetString(8),
            Status = Enum.Parse<FieldStatus>(reader.GetString(9), true),
            CreatedAt = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
            UpdatedAt = reader.IsDBNull(11) ? null : reader.GetDateTime(11)
        };
    }

    public async Task<int> CreateFieldAsync(Field field)
    {
        const string sql = @"
INSERT INTO fields (name, location, description, price_per_hour, peak_start_time, peak_end_time, peak_price_per_hour, image, status)
OUTPUT INSERTED.id
VALUES (@name, @location, @description, @pricePerHour, @peakStartTime, @peakEndTime, @peakPricePerHour, @image, @status)";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@name", field.Name);
        command.Parameters.AddWithValue("@location", field.Location);
        command.Parameters.AddWithValue("@description", field.Description ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@pricePerHour", field.PricePerHour);
        command.Parameters.AddWithValue("@peakStartTime", field.PeakStartTime.HasValue ? field.PeakStartTime.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@peakEndTime", field.PeakEndTime.HasValue ? field.PeakEndTime.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@peakPricePerHour", field.PeakPricePerHour.HasValue ? field.PeakPricePerHour.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@image", field.Image ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@status", field.Status.ToString().ToLowerInvariant());

        await connection.OpenAsync();
        var insertedId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(insertedId);
    }

    public async Task UpdateFieldAsync(Field field)
    {
        const string sql = @"
UPDATE fields
SET name = @name,
    location = @location,
    description = @description,
    price_per_hour = @pricePerHour,
    peak_start_time = @peakStartTime,
    peak_end_time = @peakEndTime,
    peak_price_per_hour = @peakPricePerHour,
    image = @image,
    status = @status,
    updated_at = GETDATE()
WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", field.Id);
        command.Parameters.AddWithValue("@name", field.Name);
        command.Parameters.AddWithValue("@location", field.Location);
        command.Parameters.AddWithValue("@description", field.Description ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@pricePerHour", field.PricePerHour);
        command.Parameters.AddWithValue("@peakStartTime", field.PeakStartTime.HasValue ? field.PeakStartTime.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@peakEndTime", field.PeakEndTime.HasValue ? field.PeakEndTime.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@peakPricePerHour", field.PeakPricePerHour.HasValue ? field.PeakPricePerHour.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@image", field.Image ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@status", field.Status.ToString().ToLowerInvariant());

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteFieldAsync(int id)
    {
        const string sql = @"DELETE FROM fields WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateFieldPriceAsync(int id, decimal pricePerHour)
    {
        const string sql = @"UPDATE fields SET price_per_hour = @pricePerHour, updated_at = GETDATE() WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@pricePerHour", pricePerHour);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateFieldPeakPricingAsync(int id, TimeSpan? peakStartTime, TimeSpan? peakEndTime, decimal? peakPricePerHour)
    {
        const string sql = @"
UPDATE fields
SET peak_start_time = @peakStartTime,
    peak_end_time = @peakEndTime,
    peak_price_per_hour = @peakPricePerHour,
    updated_at = GETDATE()
WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@peakStartTime", peakStartTime.HasValue ? peakStartTime.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@peakEndTime", peakEndTime.HasValue ? peakEndTime.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@peakPricePerHour", peakPricePerHour.HasValue ? peakPricePerHour.Value : (object)DBNull.Value);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}
