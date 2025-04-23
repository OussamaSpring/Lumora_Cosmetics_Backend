using Domain.Entities;
using Npgsql;

namespace Infrastructure.Persistence.Repositories;

public class ShoppingCartRepository
{
    private readonly IDbContext _dbContext;
    public ShoppingCartRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateShoppingCartAsync(Guid customerId)
    {
        using var connection = _dbContext.CreateConnection();
        await connection.OpenAsync();

        const string sql = @"INSERT INTO shopping_cart (customerId)
                            VALUES (@CustomerId) RETURNING id";
       
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CustomerId", customerId);
        return (int)await command.ExecuteScalarAsync();
    }

    //public async Task<int> GetShoppingCartAsync(Guid customerId)
    //{
    //    using var connection = _dbContext.CreateConnection();
    //    await connection.OpenAsync();

    //    const string sql = @"SELECT id
    //                        FROM shopping_cart
    //                        WHERE customer_id = @CustomerId";

    //    using var command = new NpgsqlCommand(sql, connection);
    //    command.Parameters.AddWithValue("@CustomerId", customerId);

    //    using var reader = await command.ExecuteReaderAsync();
    //    if (await reader.ReadAsync())
    //    {
    //        int shoppingCartId = reader.GetInt32(0);

    //        const string sql1 = @"SELECT // somethink
    //                            FROM shopping_cart_item sci_i
    //                            JOIN product_item pi ON pi.id = sci.product_item_id";

    //        using var command1 = new NpgsqlCommand(sql1, connection);
    //        command1.Parameters.AddWithValue("@CustomerId", customerId);

    //    }
    //}

    public async Task AddShoppingCartItemAsync(int shoppingCartId, ShoppingCartItem shoppingCartItem)
    {
        using var connection = _dbContext.CreateConnection();
        await connection.OpenAsync();

        const string sql = @"INSERT INTO shopping_cart_item
                            (shopping_cart_id, product_item_id, quantity)
                            VALUES (@ShoppingCartId, @ProductItemId, @Quantity)";

        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ShoppingCartId", shoppingCartItem.ShoppingCartId);
        command.Parameters.AddWithValue("@ProductItemId", shoppingCartItem.ProductItemId);
        command.Parameters.AddWithValue("@Quantity", shoppingCartItem.Quantity);
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteShoppingCartItemAsync(int shoppingCartId, int productItemId)
    {
        using var connection = _dbContext.CreateConnection();
        await connection.OpenAsync();

        const string sql = @"DELETE FROM shopping_cart_item
                            WHERE shopping_cart_id = @ShoppingCartId
                            AND product_item_id = @ProductItemId";

        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ShoppingCartId", shoppingCartId);
        command.Parameters.AddWithValue("@ProductItemId", productItemId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateShoppingCartItemAsync(int shoppingCartId, int productItemId, int quantity)
    {
        using var connection = _dbContext.CreateConnection();
        await connection.OpenAsync();

        const string sql = @"UPDATE INTO shopping_cart_item
                            SET quantity = @Quantity
                            WHERE shopping_cart_id = @ShoppingCartId
                            AND product_item_id = @ProductItemId";

        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Quantity", quantity);
        command.Parameters.AddWithValue("@ShoppingCartId", shoppingCartId);
        command.Parameters.AddWithValue("@ProductItemId", productItemId);
        await command.ExecuteNonQueryAsync();
    }
}
