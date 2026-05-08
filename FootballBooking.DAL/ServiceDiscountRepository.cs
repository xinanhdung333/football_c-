using FootballBooking.Models;
using Microsoft.Data.SqlClient;

namespace FootballBooking.DAL;

public class ServiceDiscountRepository
{
    public async Task<IEnumerable<ServiceDiscount>> GetAllAsync()
    {
        const string sql = @"
SELECT id, service_id, start_time, end_time, multiplier, note, is_active, created_at, updated_at
FROM service_discounts
ORDER BY is_active DESC, start_time";

        var discounts = new List<ServiceDiscount>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            discounts.Add(ReadDiscount(reader));
        }

        return discounts;
    }

    public async Task<IEnumerable<ServiceDiscount>> GetActiveForServiceAsync(int serviceId, TimeSpan atTime)
    {
        const string sql = @"
SELECT id, service_id, start_time, end_time, multiplier, note, is_active, created_at, updated_at
FROM service_discounts
WHERE is_active = 1
  AND (service_id IS NULL OR service_id = @serviceId)
  AND start_time <= @atTime
  AND end_time > @atTime
ORDER BY multiplier ASC, service_id DESC";

        var discounts = new List<ServiceDiscount>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@serviceId", serviceId);
        command.Parameters.AddWithValue("@atTime", atTime);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            discounts.Add(ReadDiscount(reader));
        }

        return discounts;
    }

    public async Task<int> CreateAsync(ServiceDiscount discount)
    {
        const string sql = @"
INSERT INTO service_discounts (service_id, start_time, end_time, multiplier, note, is_active)
OUTPUT INSERTED.id
VALUES (@serviceId, @startTime, @endTime, @multiplier, @note, @isActive)";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@serviceId", discount.ServiceId.HasValue ? discount.ServiceId.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@startTime", discount.StartTime);
        command.Parameters.AddWithValue("@endTime", discount.EndTime);
        command.Parameters.AddWithValue("@multiplier", discount.Multiplier);
        command.Parameters.AddWithValue("@note", discount.Note ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@isActive", discount.IsActive);

        await connection.OpenAsync();
        var insertedId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(insertedId);
    }

    public async Task DeleteAsync(int id)
    {
        const string sql = "DELETE FROM service_discounts WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    private static ServiceDiscount ReadDiscount(SqlDataReader reader)
    {
        return new ServiceDiscount
        {
            Id = reader.GetInt32(0),
            ServiceId = reader.IsDBNull(1) ? null : reader.GetInt32(1),
            StartTime = reader.GetTimeSpan(2),
            EndTime = reader.GetTimeSpan(3),
            Multiplier = reader.GetDecimal(4),
            Note = reader.IsDBNull(5) ? null : reader.GetString(5),
            IsActive = reader.GetBoolean(6),
            CreatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
            UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
        };
    }
}
