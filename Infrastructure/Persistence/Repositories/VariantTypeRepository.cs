using Application.Interfaces.Repositories;
using Domain.Entities.ProductRelated;
using Npgsql;

namespace Infrastructure.Persistence.Repositories;

public class VariantTypeRepository : IVariantTypeRepository
{
    private readonly IDbContext _dbContext;
    public VariantTypeRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddVariantTypeAsync(VariantType variant)
    {
        using var connection = _dbContext.CreateConnection();
        const string sql = @"INSERT INTO variant_type (name, description, category_id)
                            VALUES (@Name, @Description, @CategoryId) RETURNING id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Name", variant.Name);
        command.Parameters.AddWithValue("@Description", 
            variant.Description is null ? DBNull.Value : variant.Description);
        command.Parameters.AddWithValue("@CategoryId", variant.CategoryId);

        return (short)await command.ExecuteScalarAsync();
    }

    public async Task DeleteVariantTypeAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();
        const string sql = @"DELETE FROM variant_type WHERE id = @Id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<VariantType?> GetVariantTypeByIdAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();
        const string sql = @"SELECT id, name, description, category_id
                            FROM variant_type
                            WHERE id = @Id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);

        var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new VariantType
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetValue(2) == DBNull.Value ? null : reader.GetString(2),
                CategoryId = reader.GetInt32(3)
            };
        } 
        return null;
    }

    public async Task<IEnumerable<VariantType>> GetVariantTypeForCategory(int categoryId)
    {
        using var connection = _dbContext.CreateConnection();
        const string sql = @"SELECT v.id, v.name, v.description, v.category_id
                            FROM variant_type v JOIN category c ON v.category_id = c.id
                            WHERE c.id = @Id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", categoryId);

        var reader = await command.ExecuteReaderAsync();
        List<VariantType> variants = new List<VariantType>();
        while (await reader.ReadAsync())
        {
            variants.Add(new VariantType
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetValue(2) == DBNull.Value ? null : reader.GetString(2),
                CategoryId = reader.GetInt32(3)
            });
        }
        return variants;
    }

    public async Task UpdateVariantTypeAsync(int variantTypeId, VariantType variant)
    {
        using var connection = _dbContext.CreateConnection();
        const string sql = @"UPDATE variant_type
                            SET name = @Name, description = @Description
                            WHERE id = @Id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Name", variant.Name);
        command.Parameters.AddWithValue("@Description",
            variant.Description is null ? DBNull.Value : variant.Description);
        command.Parameters.AddWithValue("@Id", variantTypeId);

        await command.ExecuteNonQueryAsync();
    }
}
