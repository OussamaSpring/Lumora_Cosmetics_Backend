using Npgsql;
using Application.Interfaces.Repositories;
using Domain.Entities.ProductRelated;

namespace Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbContext _databaseContext;
    public CategoryRepository(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Category?> GetCategoryByIdAsync(int categoryId)
    {
        using var connection = _databaseContext.CreateConnection();
        const string sql = @"
                SELECT id, name, description
                FROM category
                WHERE id = @categoryId";

        await connection.OpenAsync();
        using var categoryCommand = new NpgsqlCommand(sql, connection);
        categoryCommand.Parameters.AddWithValue("@categoryId", categoryId);

        using var reader = await categoryCommand.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Category
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2)
            };
        }

        return null;
    }

    public async Task<IEnumerable<VariantType>> GetVariantTypesForCategoryAsync(int categoryId)
    {
        var variantTypes = new List<VariantType>();

        using var connection = _databaseContext.CreateConnection();

        await connection.OpenAsync();

        // Get all parent categories in hierarchy
        var categoryHierarchy = new List<int>();
        var currentCategoryId = categoryId;


        // Get all parent categories IDs in hierarchy
        while (currentCategoryId != 0)
        {
            categoryHierarchy.Add(currentCategoryId);

            const string parentQuery = "SELECT parent_id FROM category WHERE id = @categoryId";


            using var parentCommand = new NpgsqlCommand(parentQuery, connection);


            parentCommand.Parameters.AddWithValue("@categoryId", currentCategoryId);

            currentCategoryId = (int)(await parentCommand.ExecuteScalarAsync() ?? 0);
        }

        // get variant types for all categories in hierarchy
        const string query = @"
                SELECT id, name, description, category_id
                FROM variant_type
                WHERE category_id = ANY(@categoryIds)";

        using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("@categoryIds", categoryHierarchy);

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            variantTypes.Add(new VariantType
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                CategoryId = reader.GetInt16(3)
            });
        }

        return variantTypes;
    }

    public async Task<int> CreateAsync(Category category)
    {
        using var connection = _databaseContext.CreateConnection();
        const string sql = @"INSERT INTO category (name, description) VALUES (@Name, @Description) RETURNING id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Name", category.Name);
        command.Parameters.AddWithValue("@Description",
            category.Description is null ? DBNull.Value : category.Description);

        return (short)await command.ExecuteScalarAsync();
    }

    public async Task<Category?> DeleteAsync(int categoryId)
    {
        var category = await GetCategoryByIdAsync(categoryId);

        using var connection = _databaseContext.CreateConnection();
        const string sql = @"DELETE FROM category WHERE id = @Id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", categoryId);
        await command.ExecuteNonQueryAsync();

        return category;
    }

    public async Task UpdateCategoryAsync(int categoryId, Category category)
    {
        using var connection = _databaseContext.CreateConnection();
        const string sql = @"UPDATE category
                            SET name = @Name, description = @Description
                            WHERE id = @Id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Name", category.Name);
        command.Parameters.AddWithValue("@Description",
            category.Description is null ? DBNull.Value : category.Description);
        command.Parameters.AddWithValue("@Id", categoryId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        using var connection = _databaseContext.CreateConnection();
        const string sql = @"SELECT id, name, description FROM category";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sql, connection);
        var reader = await command.ExecuteReaderAsync();

        var categories = new List<Category>();
        while (await reader.ReadAsync())
        {
            categories.Add(new Category
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2)
            });
        }
        return categories;
    }

    public async Task<IEnumerable<short>?> GetChildCategoriesIDsAsync(List<short> ParentIDs)
    {
        using var connection = _databaseContext.CreateConnection();

        const string sql = @"SELECT id FROM category WHERE parent_id = ANY (@ParentID)";


        await connection.OpenAsync();

        using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("@ParentID", ParentIDs.ToArray());

        using var reader = await command.ExecuteReaderAsync();

        var categoryIDs = new List<short>();

        while (await reader.ReadAsync())
        {
            categoryIDs.Add(reader.GetInt16(0));
        }
        return categoryIDs;
    }
}
