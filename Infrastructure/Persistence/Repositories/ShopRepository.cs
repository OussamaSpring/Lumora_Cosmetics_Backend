using Application.Interfaces.Repositories;
using Domain.Entities.ShopRelated;
using Npgsql;

namespace Infrastructure.Persistence.Repositories;

public class ShopRepository : IShopRepository
{
    private readonly IDbContext _databaseContext;
    public ShopRepository(IDbContext databaseContext)
{
    _databaseContext = databaseContext;
}

    public async Task<Shop?> GetShopByVendorIdAsync(Guid vendorId)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        var query = "SELECT * FROM Shops WHERE VendorId = @VendorId";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@VendorId", vendorId);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Shop
            {
                Id = (int)reader["Id"],
                VendorId = (Guid)reader["VendorId"],
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader["Description"]?.ToString(),
                LogoUrl = reader["LogoUrl"]?.ToString(),
                MapAddress = reader["MapAddress"]?.ToString(),
                CreateDate = (DateTime)reader["CreateDate"],
                UpdateDate = (DateTime)reader["UpdateDate"]
            };
        }
        return null;
    }

    public async Task<int> CreateShopAsync(Shop shop)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        var query = @"
                INSERT INTO Shops (VendorId, Name, Description, LogoUrl, MapAddress, CreateDate, UpdateDate)
                VALUES (@VendorId, @Name, @Description, @LogoUrl, @MapAddress, @CreateDate, @UpdateDate) RETURNING id;";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@VendorId", shop.VendorId);
        command.Parameters.AddWithValue("@Name", shop.Name);
        command.Parameters.AddWithValue("@Description", (object?)shop.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@LogoUrl", (object?)shop.LogoUrl ?? DBNull.Value);
        command.Parameters.AddWithValue("@MapAddress", (object?)shop.MapAddress ?? DBNull.Value);
        command.Parameters.AddWithValue("@CreateDate", shop.CreateDate);
        command.Parameters.AddWithValue("@UpdateDate", shop.UpdateDate);

        return (int)await command.ExecuteScalarAsync();
    }

    public async Task UpdateShopAsync(Shop shop)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        var query = @"
                UPDATE Shops SET 
                    Name = @Name,
                    Description = @Description,
                    LogoUrl = @LogoUrl,
                    MapAddress = @MapAddress,
                    UpdateDate = @UpdateDate
                WHERE VendorId = @VendorId";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@Name", shop.Name);
        command.Parameters.AddWithValue("@Description", (object?)shop.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@LogoUrl", (object?)shop.LogoUrl ?? DBNull.Value);
        command.Parameters.AddWithValue("@MapAddress", (object?)shop.MapAddress ?? DBNull.Value);
        command.Parameters.AddWithValue("@UpdateDate", shop.UpdateDate);
        command.Parameters.AddWithValue("@VendorId", shop.VendorId);

        await command.ExecuteNonQueryAsync();
    }

}