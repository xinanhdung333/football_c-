using FootballBooking.Models;
using Microsoft.Data.SqlClient;

namespace FootballBooking.DAL;

public class PaymentRepository
{
    public async Task<int> CreatePaymentAsync(Payment payment)
    {
        await using var connection = SqlConnectionFactory.CreateConnection();
        await connection.OpenAsync();
        return await CreatePaymentAsync(payment, connection, null);
    }

    public async Task<int> CreatePaymentAsync(Payment payment, SqlConnection connection, SqlTransaction? transaction)
    {
        const string sql = @"
INSERT INTO payments (order_id, momo_order_id, momo_trans_id, amount, status, paid_at)
OUTPUT INSERTED.id
VALUES (@orderId, @momoOrderId, @momoTransId, @amount, @status, @paidAt)";

        await using var command = new SqlCommand(sql, connection, transaction);

        command.Parameters.AddWithValue("@orderId", payment.OrderId);
        command.Parameters.AddWithValue("@momoOrderId", payment.MomoOrderId ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@momoTransId", payment.MomoTransId ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@amount", payment.Amount);
        command.Parameters.AddWithValue("@status", payment.Status.ToString().ToLowerInvariant());
        command.Parameters.AddWithValue("@paidAt", payment.PaidAt.HasValue ? payment.PaidAt.Value : (object)DBNull.Value);

        var insertedId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(insertedId);
    }
}
