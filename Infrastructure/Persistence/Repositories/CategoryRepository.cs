
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





    #region GetData

    /* This function is not efficient as it makes multiple database calls to get the category
     * and its parent categories, and have repeated code so much. But it works!
     * Don't worry about it for now.
    */
    public async Task<Category?> GetCategoryByIdAsync(int categoryId)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        // First get the category
        const string categoryQuery = @"
                SELECT id, name, description, parent_id
                FROM category
                WHERE id = @categoryId";

        using var categoryCommand = new NpgsqlCommand(categoryQuery, connection);
        categoryCommand.Parameters.AddWithValue("@categoryId", categoryId);

        Category? category = null;
        int? parent1Id = null;

        using var reader = await categoryCommand.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            category = new Category
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2)
            };
            parent1Id = reader.IsDBNull(3) ? null : reader.GetInt16(3);
        }

        if (category == null) // get rid of the warning
        {
            return null;
        }


        int? parent2Id = null;

        // if the category has a parent, get the parent category
        if (parent1Id.HasValue)
        {
            const string parentQuery = @"
                    SELECT id, name, description, parent_id
                    FROM category
                    WHERE id = @parent1Id";

            using var parentCommand = new NpgsqlCommand(parentQuery, connection);
            parentCommand.Parameters.AddWithValue("@parent1Id", parent1Id);

            using var parentReader = await parentCommand.ExecuteReaderAsync();
            if (await parentReader.ReadAsync())
            {
                category.Parent = new Category
                {
                    Id = parentReader.GetInt32(0),
                    Name = parentReader.GetString(1),
                    Description = parentReader.IsDBNull(2) ? null : parentReader.GetString(2)
                };
                parent2Id = parentReader.IsDBNull(3) ? null : parentReader.GetInt16(3);
            }
        }

        if (category.Parent == null)
        {
            return null;
        }

        if (parent2Id.HasValue)
        {
            const string parentQuery = @"
                    SELECT id, name, description
                    FROM category
                    WHERE id = @parent2Id";

            using var parentCommand = new NpgsqlCommand(parentQuery, connection);
            parentCommand.Parameters.AddWithValue("@parent2Id", parent2Id);

            using var parentReader = await parentCommand.ExecuteReaderAsync();
            if (await parentReader.ReadAsync())
            {
                category.Parent.Parent = new Category
                {
                    Id = parentReader.GetInt32(0),
                    Name = parentReader.GetString(1),
                    Description = parentReader.IsDBNull(2) ? null : parentReader.GetString(2)
                };
            }
        }

        return category;
    }


    // this is optimized function then above GetCategoryByIdAsync
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


    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        // First get all categories
        const string query = @"
            SELECT id, name, description, parent_id
            FROM category
            ORDER BY parent_id NULLS FIRST, name";

        using var command = new NpgsqlCommand(query, connection);

        var categories = new Dictionary<int, Category>();
        var parentRelations = new Dictionary<int, int?>();

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var category = new Category
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2)
            };

            categories[category.Id] = category;
            parentRelations[category.Id] = reader.IsDBNull(3) ? null : reader.GetInt32(3);
        }

        // Build parent relationships
        foreach (var kvp in parentRelations)
        {
            if (kvp.Value.HasValue && categories.TryGetValue(kvp.Value.Value, out var parent))
            {
                categories[kvp.Key].Parent = parent;
            }
        }

        return categories.Values;
    }







    #endregion
}
