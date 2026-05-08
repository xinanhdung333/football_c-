using FootballBooking.Models;
using Microsoft.Data.SqlClient;

namespace FootballBooking.DAL;

public class CartRepository
{
    public async Task<Cart?> GetCartByUserIdAsync(int userId)
    {
        const string sql = @"SELECT id, user_id, created_at, updated_at FROM cart WHERE user_id = @userId";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new Cart
        {
            Id = reader.GetInt32(0),
            UserId = reader.GetInt32(1),
            CreatedAt = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
            UpdatedAt = reader.IsDBNull(3) ? null : reader.GetDateTime(3)
        };
    }

    public async Task<int> CreateCartAsync(int userId)
    {
        const string sql = @"INSERT INTO cart (user_id) OUTPUT INSERTED.id VALUES (@userId)";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);

        await connection.OpenAsync();
        var insertedId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(insertedId);
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId)
    {
        const string sql = @"SELECT id, cart_id, service_id, quantity, price, created_at, updated_at FROM cart_items WHERE cart_id = @cartId";

        var items = new List<CartItem>();
        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@cartId", cartId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            items.Add(new CartItem
            {
                Id = reader.GetInt32(0),
                CartId = reader.GetInt32(1),
                ServiceId = reader.GetInt32(2),
                Quantity = reader.GetInt32(3),
                Price = reader.GetDecimal(4),
                CreatedAt = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
            });
        }

        return items;
    }

    public async Task AddCartItemAsync(CartItem item)
    {
        const string sql = @"INSERT INTO cart_items (cart_id, service_id, quantity, price) VALUES (@cartId, @serviceId, @quantity, @price)";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@cartId", item.CartId);
        command.Parameters.AddWithValue("@serviceId", item.ServiceId);
        command.Parameters.AddWithValue("@quantity", item.Quantity);
        command.Parameters.AddWithValue("@price", item.Price);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<CartItem?> GetCartItemByServiceIdAsync(int cartId, int serviceId)
    {
        const string sql = @"SELECT id, cart_id, service_id, quantity, price, created_at, updated_at FROM cart_items WHERE cart_id = @cartId AND service_id = @serviceId";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@cartId", cartId);
        command.Parameters.AddWithValue("@serviceId", serviceId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new CartItem
        {
            Id = reader.GetInt32(0),
            CartId = reader.GetInt32(1),
            ServiceId = reader.GetInt32(2),
            Quantity = reader.GetInt32(3),
            Price = reader.GetDecimal(4),
            CreatedAt = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
            UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
        };
    }

    public async Task<CartItem?> GetCartItemByIdAsync(int itemId)
    {
        const string sql = @"SELECT id, cart_id, service_id, quantity, price, created_at, updated_at FROM cart_items WHERE id = @itemId";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@itemId", itemId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new CartItem
        {
            Id = reader.GetInt32(0),
            CartId = reader.GetInt32(1),
            ServiceId = reader.GetInt32(2),
            Quantity = reader.GetInt32(3),
            Price = reader.GetDecimal(4),
            CreatedAt = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
            UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
        };
    }

    public async Task UpdateCartItemQuantityAsync(int cartItemId, int quantity)
    {
        const string sql = @"UPDATE cart_items SET quantity = @quantity, updated_at = GETDATE() WHERE id = @itemId";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@itemId", cartItemId);
        command.Parameters.AddWithValue("@quantity", quantity);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task RemoveCartItemAsync(int cartItemId)
    {
        const string sql = @"DELETE FROM cart_items WHERE id = @itemId";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@itemId", cartItemId);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task ClearCartAsync(int cartId)
    {
        const string sql = @"DELETE FROM cart_items WHERE cart_id = @cartId";

        await using var connection = SqlConnectionFactory.CreateConnection();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@cartId", cartId);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}