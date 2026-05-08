using FootballBooking.Models;
using Microsoft.Data.SqlClient;

namespace FootballBooking.DAL;

public class OrderRepository
{
    public async Task<int> CreateOrderAsync(Order order)
    {
        await using var connection = SqlConnectionFactory.CreateConnection();
        await connection.OpenAsync();
        return await CreateOrderAsync(order, connection, null);
    }

    public async Task<int> CreateOrderAsync(Order order, SqlConnection connection, SqlTransaction? transaction)
    {
        const string sql = @"
INSERT INTO orders (user_id, cart_id, total_amount, payment_method, status)
OUTPUT INSERTED.id
VALUES (@userId, @cartId, @totalAmount, @paymentMethod, @status)";

        await using var command = new SqlCommand(sql, connection, transaction);

        command.Parameters.AddWithValue("@userId", order.UserId);
        command.Parameters.AddWithValue("@cartId", order.CartId.HasValue ? order.CartId.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@totalAmount", order.TotalAmount);
        command.Parameters.AddWithValue("@paymentMethod", order.PaymentMethod ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@status", order.Status.ToString().ToLowerInvariant());

        var insertedId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(insertedId);
    }

    public async Task CreateOrderItemAsync(OrderItem item)
    {
        await using var connection = SqlConnectionFactory.CreateConnection();
        await connection.OpenAsync();
        await CreateOrderItemAsync(item, connection, null);
    }

    public async Task CreateOrderItemAsync(OrderItem item, SqlConnection connection, SqlTransaction? transaction)
    {
        const string sql = @"
INSERT INTO order_items (order_id, service_id, quantity, price, status)
VALUES (@orderId, @serviceId, @quantity, @price, @status)";

        await using var command = new SqlCommand(sql, connection, transaction);

        command.Parameters.AddWithValue("@orderId", item.OrderId);
        command.Parameters.AddWithValue("@serviceId", item.ServiceId);
        command.Parameters.AddWithValue("@quantity", item.Quantity);
        command.Parameters.AddWithValue("@price", item.Price);
        command.Parameters.AddWithValue("@status", item.Status.ToString().ToLowerInvariant());

        await command.ExecuteNonQueryAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        const string sql = @"
SELECT id, user_id, cart_id, total_amount, payment_method, status, created_at
FROM orders
WHERE id = @orderId";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@orderId", orderId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Order
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                CartId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                TotalAmount = reader.GetDecimal(3),
                PaymentMethod = reader.IsDBNull(4) ? null : reader.GetString(4),
                Status = Enum.Parse<OrderStatus>(reader.GetString(5), true),
                CreatedAt = reader.GetDateTime(6)
            };
        }

        return null;
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
    {
        const string sql = @"
SELECT id, user_id, cart_id, total_amount, payment_method, status, created_at
FROM orders
WHERE user_id = @userId
ORDER BY created_at DESC";

        var orders = new List<Order>();

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@userId", userId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            orders.Add(new Order
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                CartId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                TotalAmount = reader.GetDecimal(3),
                PaymentMethod = reader.IsDBNull(4) ? null : reader.GetString(4),
                Status = Enum.Parse<OrderStatus>(reader.GetString(5), true),
                CreatedAt = reader.GetDateTime(6)
            });
        }

        return orders;
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
    {
        const string sql = @"
SELECT id, order_id, service_id, quantity, price, status
FROM order_items
WHERE order_id = @orderId";

        var items = new List<OrderItem>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@orderId", orderId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            items.Add(new OrderItem
            {
                Id = reader.GetInt32(0),
                OrderId = reader.GetInt32(1),
                ServiceId = reader.GetInt32(2),
                Quantity = reader.GetInt32(3),
                Price = reader.GetDecimal(4),
                Status = Enum.Parse<OrderStatus>(reader.GetString(5), true)
            });
        }

        return items;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        const string sql = @"
SELECT id, user_id, cart_id, total_amount, payment_method, status, created_at
FROM orders
ORDER BY created_at DESC";

        var orders = new List<Order>();

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            orders.Add(new Order
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                CartId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                TotalAmount = reader.GetDecimal(3),
                PaymentMethod = reader.IsDBNull(4) ? null : reader.GetString(4),
                Status = Enum.Parse<OrderStatus>(reader.GetString(5), true),
                CreatedAt = reader.GetDateTime(6)
            });
        }

        return orders;
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        const string sql = @"
UPDATE orders
SET status = @status
WHERE id = @orderId";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@status", status.ToString().ToLowerInvariant());
        command.Parameters.AddWithValue("@orderId", orderId);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}
