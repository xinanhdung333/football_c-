using FootballBooking.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FootballBooking.DAL;

public class BookingRepository
{
    public async Task<bool> HasScheduleConflictAsync(int fieldId, DateTime bookingDate, TimeSpan startTime, TimeSpan endTime)
    {
        const string sql = @"
SELECT COUNT(*)
FROM bookings
WHERE field_id = @fieldId
  AND booking_date = @bookingDate
  AND NOT (end_time <= @startTime OR start_time >= @endTime)";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@fieldId", fieldId);
        command.Parameters.AddWithValue("@bookingDate", bookingDate.Date);
        command.Parameters.AddWithValue("@startTime", startTime);
        command.Parameters.AddWithValue("@endTime", endTime);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        var count = result is null or DBNull ? 0 : Convert.ToInt32(result);
        return count > 0;
    }

    public async Task<int> CreateBookingAsync(Booking booking)
    {
        const string sql = @"
INSERT INTO bookings (user_id, field_id, booking_date, start_time, end_time, total_price, status, note)
OUTPUT INSERTED.id
VALUES (@userId, @fieldId, @bookingDate, @startTime, @endTime, @totalPrice, @status, @note)";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@userId", booking.UserId);
        command.Parameters.AddWithValue("@fieldId", booking.FieldId);
        command.Parameters.AddWithValue("@bookingDate", booking.BookingDate.Date);
        command.Parameters.AddWithValue("@startTime", booking.StartTime);
        command.Parameters.AddWithValue("@endTime", booking.EndTime);
        command.Parameters.AddWithValue("@totalPrice", booking.TotalPrice);
        command.Parameters.AddWithValue("@status", booking.Status.ToString().ToLowerInvariant());
        command.Parameters.AddWithValue("@note", booking.Note ?? (object)DBNull.Value);

        await connection.OpenAsync();
        var insertedId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(insertedId);
    }

    public async Task<Booking?> GetBookingByIdAsync(int id)
    {
        const string sql = @"
SELECT id, user_id, field_id, booking_date, start_time, end_time, total_price, status, note, created_at, updated_at
FROM bookings
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

        return new Booking
        {
            Id = reader.GetInt32(0),
            UserId = reader.GetInt32(1),
            FieldId = reader.GetInt32(2),
            BookingDate = reader.GetDateTime(3),
            StartTime = reader.GetTimeSpan(4),
            EndTime = reader.GetTimeSpan(5),
            TotalPrice = reader.GetDecimal(6),
            Status = Enum.Parse<BookingStatus>(reader.GetString(7), true),
            Note = reader.IsDBNull(8) ? null : reader.GetString(8),
            CreatedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
            UpdatedAt = reader.IsDBNull(10) ? null : reader.GetDateTime(10)
        };
    }

    public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
    {
        const string sql = @"
SELECT id, user_id, field_id, booking_date, start_time, end_time, total_price, status, note, created_at, updated_at
FROM bookings
WHERE user_id = @userId
ORDER BY booking_date DESC";

        var bookings = new List<Booking>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            bookings.Add(new Booking
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                FieldId = reader.GetInt32(2),
                BookingDate = reader.GetDateTime(3),
                StartTime = reader.GetTimeSpan(4),
                EndTime = reader.GetTimeSpan(5),
                TotalPrice = reader.GetDecimal(6),
                Status = Enum.Parse<BookingStatus>(reader.GetString(7), true),
                Note = reader.IsDBNull(8) ? null : reader.GetString(8),
                CreatedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                UpdatedAt = reader.IsDBNull(10) ? null : reader.GetDateTime(10)
            });
        }

        return bookings;
    }

    public async Task UpdateBookingAsync(Booking booking)
    {
        const string sql = @"
UPDATE bookings
SET user_id = @userId,
    field_id = @fieldId,
    booking_date = @bookingDate,
    start_time = @startTime,
    end_time = @endTime,
    total_price = @totalPrice,
    status = @status,
    note = @note,
    updated_at = GETDATE()
WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", booking.Id);
        command.Parameters.AddWithValue("@userId", booking.UserId);
        command.Parameters.AddWithValue("@fieldId", booking.FieldId);
        command.Parameters.AddWithValue("@bookingDate", booking.BookingDate.Date);
        command.Parameters.AddWithValue("@startTime", booking.StartTime);
        command.Parameters.AddWithValue("@endTime", booking.EndTime);
        command.Parameters.AddWithValue("@totalPrice", booking.TotalPrice);
        command.Parameters.AddWithValue("@status", booking.Status.ToString().ToLowerInvariant());
        command.Parameters.AddWithValue("@note", booking.Note ?? (object)DBNull.Value);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteBookingAsync(int id)
    {
        const string sql = @"DELETE FROM bookings WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
    {
        const string sql = @"
SELECT id, user_id, field_id, booking_date, start_time, end_time, total_price, status, note, created_at, updated_at
FROM bookings
ORDER BY created_at DESC";

        var bookings = new List<Booking>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            bookings.Add(new Booking
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                FieldId = reader.GetInt32(2),
                BookingDate = reader.GetDateTime(3),
                StartTime = reader.GetTimeSpan(4),
                EndTime = reader.GetTimeSpan(5),
                TotalPrice = reader.GetDecimal(6),
                Status = Enum.Parse<BookingStatus>(reader.GetString(7), true),
                Note = reader.IsDBNull(8) ? null : reader.GetString(8),
                CreatedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                UpdatedAt = reader.IsDBNull(10) ? null : reader.GetDateTime(10)
            });
        }

        return bookings;
    }

    public async Task<IEnumerable<BookingWithDetails>> GetAllBookingsWithDetailsAsync()
    {
        const string sql = @"
SELECT b.id, b.user_id, b.field_id, b.booking_date, b.start_time, b.end_time, b.total_price, b.status, b.note, b.created_at, b.updated_at,
       u.name as user_name, u.email as user_email,
       f.name as field_name, f.location as field_location
FROM bookings b
INNER JOIN users u ON b.user_id = u.id
INNER JOIN fields f ON b.field_id = f.id
ORDER BY b.created_at DESC";

        var bookings = new List<BookingWithDetails>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            bookings.Add(new BookingWithDetails
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                FieldId = reader.GetInt32(2),
                BookingDate = reader.GetDateTime(3),
                StartTime = reader.GetTimeSpan(4),
                EndTime = reader.GetTimeSpan(5),
                TotalPrice = reader.GetDecimal(6),
                Status = Enum.Parse<BookingStatus>(reader.GetString(7), true),
                Note = reader.IsDBNull(8) ? null : reader.GetString(8),
                CreatedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                UpdatedAt = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                UserName = reader.GetString(11),
                UserEmail = reader.GetString(12),
                FieldName = reader.GetString(13),
                FieldLocation = reader.GetString(14)
            });
        }

        return bookings;
    }
}
