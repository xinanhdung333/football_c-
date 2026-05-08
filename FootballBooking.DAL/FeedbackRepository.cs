using FootballBooking.Models;
using Microsoft.Data.SqlClient;

namespace FootballBooking.DAL;

public class FeedbackRepository
{
    public async Task CreateFeedbackAsync(Feedback feedback)
    {
        const string sql = @"INSERT INTO feedbacks (user_id, booking_id, service_id, message, rating) VALUES (@userId, @bookingId, @serviceId, @message, @rating)";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@userId", feedback.UserId);
        command.Parameters.AddWithValue("@bookingId", feedback.BookingId.HasValue ? feedback.BookingId.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@serviceId", feedback.ServiceId.HasValue ? feedback.ServiceId.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@message", feedback.Message);
        command.Parameters.AddWithValue("@rating", feedback.Rating);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
    {
        const string sql = @"SELECT id, user_id, booking_id, service_id, message, rating, created_at, reply_message, replied_by, replied_at FROM feedbacks ORDER BY created_at DESC";

        var feedbacks = new List<Feedback>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            feedbacks.Add(new Feedback
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                BookingId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                ServiceId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                Message = reader.GetString(4),
                Rating = reader.GetByte(5),
                CreatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                ReplyMessage = reader.IsDBNull(7) ? null : reader.GetString(7),
                RepliedBy = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                RepliedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9)
            });
        }

        return feedbacks;
    }

    public async Task<IEnumerable<FeedbackWithDetails>> GetAllFeedbacksWithDetailsAsync()
    {
        const string sql = @"
SELECT f.id, f.user_id, f.booking_id, f.service_id, f.message, f.rating, f.created_at, f.reply_message, f.replied_by, f.replied_at,
       u.name as user_name, u.email as user_email,
       ru.name as replied_by_name
FROM feedbacks f
INNER JOIN users u ON f.user_id = u.id
LEFT JOIN users ru ON f.replied_by = ru.id
ORDER BY f.created_at DESC";

        var feedbacks = new List<FeedbackWithDetails>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            feedbacks.Add(new FeedbackWithDetails
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                BookingId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                ServiceId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                Message = reader.GetString(4),
                Rating = reader.GetByte(5),
                CreatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                ReplyMessage = reader.IsDBNull(7) ? null : reader.GetString(7),
                RepliedBy = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                RepliedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                UserName = reader.GetString(10),
                UserEmail = reader.GetString(11),
                RepliedByName = reader.IsDBNull(12) ? null : reader.GetString(12)
            });
        }

        return feedbacks;
    }

    public async Task<Feedback?> GetFeedbackByIdAsync(int id)
    {
        const string sql = @"SELECT id, user_id, booking_id, service_id, message, rating, created_at, reply_message, replied_by, replied_at FROM feedbacks WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new Feedback
        {
            Id = reader.GetInt32(0),
            UserId = reader.GetInt32(1),
            BookingId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
            ServiceId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
            Message = reader.GetString(4),
            Rating = reader.GetByte(5),
            CreatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
            ReplyMessage = reader.IsDBNull(7) ? null : reader.GetString(7),
            RepliedBy = reader.IsDBNull(8) ? null : reader.GetInt32(8),
            RepliedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9)
        };
    }

    public async Task UpdateFeedbackAsync(Feedback feedback)
    {
        const string sql = @"
UPDATE feedbacks
SET user_id = @userId,
    booking_id = @bookingId,
    service_id = @serviceId,
    message = @message,
    rating = @rating,
    reply_message = @replyMessage,
    replied_by = @repliedBy,
    replied_at = @repliedAt
WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", feedback.Id);
        command.Parameters.AddWithValue("@userId", feedback.UserId);
        command.Parameters.AddWithValue("@bookingId", feedback.BookingId.HasValue ? feedback.BookingId.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@serviceId", feedback.ServiceId.HasValue ? feedback.ServiceId.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@message", feedback.Message);
        command.Parameters.AddWithValue("@rating", feedback.Rating);
        command.Parameters.AddWithValue("@replyMessage", feedback.ReplyMessage ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@repliedBy", feedback.RepliedBy.HasValue ? feedback.RepliedBy.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@repliedAt", feedback.RepliedAt.HasValue ? feedback.RepliedAt.Value : (object)DBNull.Value);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteFeedbackAsync(int id)
    {
        const string sql = @"DELETE FROM feedbacks WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<Feedback>> GetFeedbacksByBookingIdAsync(int bookingId)
    {
        const string sql = @"SELECT id, user_id, booking_id, service_id, message, rating, created_at, reply_message, replied_by, replied_at FROM feedbacks WHERE booking_id = @bookingId";

        var feedbacks = new List<Feedback>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@bookingId", bookingId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            feedbacks.Add(new Feedback
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                BookingId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                ServiceId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                Message = reader.GetString(4),
                Rating = reader.GetByte(5),
                CreatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                ReplyMessage = reader.IsDBNull(7) ? null : reader.GetString(7),
                RepliedBy = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                RepliedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9)
            });
        }

        return feedbacks;
    }

    public async Task<IEnumerable<FeedbackWithField>> GetFeedbacksByUserIdAsync(int userId)
    {
        const string sql = @"
SELECT f.id,
       f.user_id,
       f.booking_id,
       f.service_id,
       f.message,
       f.rating,
       f.created_at,
       f.reply_message,
       f.replied_by,
       f.replied_at,
       fld.name AS field_name,
       ru.name AS replied_by_name
FROM feedbacks f
LEFT JOIN bookings b ON f.booking_id = b.id
LEFT JOIN fields fld ON b.field_id = fld.id
LEFT JOIN users ru ON f.replied_by = ru.id
WHERE f.user_id = @userId
ORDER BY f.created_at DESC";

        var feedbacks = new List<FeedbackWithField>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            feedbacks.Add(new FeedbackWithField
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                BookingId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                ServiceId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                Message = reader.GetString(4),
                Rating = reader.GetByte(5),
                CreatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                ReplyMessage = reader.IsDBNull(7) ? null : reader.GetString(7),
                RepliedBy = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                RepliedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                FieldName = reader.IsDBNull(10) ? null : reader.GetString(10),
                RepliedByName = reader.IsDBNull(11) ? null : reader.GetString(11)
            });
        }

        return feedbacks;
    }

    public async Task<IEnumerable<Feedback>> GetFeedbacksByServiceIdAsync(int serviceId)
    {
        const string sql = @"SELECT id, user_id, booking_id, service_id, message, rating, created_at, reply_message, replied_by, replied_at FROM feedbacks WHERE service_id = @serviceId ORDER BY created_at DESC";

        var feedbacks = new List<Feedback>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@serviceId", serviceId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            feedbacks.Add(new Feedback
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                BookingId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                ServiceId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                Message = reader.GetString(4),
                Rating = reader.GetByte(5),
                CreatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                ReplyMessage = reader.IsDBNull(7) ? null : reader.GetString(7),
                RepliedBy = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                RepliedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9)
            });
        }

        return feedbacks;
    }

    public async Task ReplyFeedbackAsync(int feedbackId, int repliedBy, string replyMessage)
    {
        const string sql = @"
UPDATE feedbacks
SET reply_message = @replyMessage,
    replied_by = @repliedBy,
    replied_at = GETDATE()
WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", feedbackId);
        command.Parameters.AddWithValue("@repliedBy", repliedBy);
        command.Parameters.AddWithValue("@replyMessage", replyMessage);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}
