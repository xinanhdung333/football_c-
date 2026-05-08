using FootballBooking.Models;
using Microsoft.Data.SqlClient;

namespace FootballBooking.DAL;

public class BookingPaymentRepository
{
    public async Task<int> CreateAsync(BookingPayment payment)
    {
        const string sql = @"
INSERT INTO booking_payments (booking_id, payment_method, momo_order_id, momo_trans_id, amount, status, paid_at)
OUTPUT INSERTED.id
VALUES (@bookingId, @paymentMethod, @momoOrderId, @momoTransId, @amount, @status, @paidAt)";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@bookingId", payment.BookingId);
        command.Parameters.AddWithValue("@paymentMethod", payment.PaymentMethod);
        command.Parameters.AddWithValue("@momoOrderId", payment.MomoOrderId ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@momoTransId", payment.MomoTransId ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@amount", payment.Amount);
        command.Parameters.AddWithValue("@status", payment.Status.ToString().ToLowerInvariant());
        command.Parameters.AddWithValue("@paidAt", payment.PaidAt.HasValue ? payment.PaidAt.Value : (object)DBNull.Value);

        await connection.OpenAsync();
        var insertedId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(insertedId);
    }

    public async Task<BookingPayment?> GetLatestByBookingIdAsync(int bookingId)
    {
        const string sql = @"
SELECT TOP 1 id, booking_id, payment_method, momo_order_id, momo_trans_id, amount, status, paid_at, created_at, updated_at
FROM booking_payments
WHERE booking_id = @bookingId
ORDER BY created_at DESC, id DESC";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@bookingId", bookingId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return ReadPayment(reader);
    }

    public async Task UpdateStatusAsync(int id, PaymentStatus status, string? momoTransId = null)
    {
        const string sql = @"
UPDATE booking_payments
SET status = @status,
    momo_trans_id = COALESCE(@momoTransId, momo_trans_id),
    paid_at = CASE WHEN @status = 'success' THEN COALESCE(paid_at, GETDATE()) ELSE paid_at END,
    updated_at = GETDATE()
WHERE id = @id";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@status", status.ToString().ToLowerInvariant());
        command.Parameters.AddWithValue("@momoTransId", momoTransId ?? (object)DBNull.Value);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    private static BookingPayment ReadPayment(SqlDataReader reader)
    {
        return new BookingPayment
        {
            Id = reader.GetInt32(0),
            BookingId = reader.GetInt32(1),
            PaymentMethod = reader.GetString(2),
            MomoOrderId = reader.IsDBNull(3) ? null : reader.GetString(3),
            MomoTransId = reader.IsDBNull(4) ? null : reader.GetString(4),
            Amount = reader.GetDecimal(5),
            Status = Enum.Parse<PaymentStatus>(reader.GetString(6), true),
            PaidAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
            CreatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
            UpdatedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9)
        };
    }
}
