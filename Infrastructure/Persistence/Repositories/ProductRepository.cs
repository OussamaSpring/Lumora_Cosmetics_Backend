using Npgsql;

using System.Text.Json;
using System.Text;


using Application.Interfaces.Repositories;
using Domain.Entities.ProductRelated;
using Domain.Enums.Product;
using Application.DTOs;


namespace Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbContext _databaseContext;

    public ProductRepository(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }


    #region HelpFunction

    private Product MapProduct(NpgsqlDataReader reader)
    {
        return new Product
        {
            Id = reader.GetInt32(0),
            ShopId = reader.GetInt32(1),
            SerialNumber = reader.IsDBNull(2) ? null : reader.GetString(2),
            Name = reader.IsDBNull(3) ? null : reader.GetString(3),
            Brand = reader.IsDBNull(4) ? null : reader.GetString(4),
            About = reader.IsDBNull(5) ? null : reader.GetString(5),
            Ingredients = reader.IsDBNull(6) ? null : reader.GetString(6),
            HowToUse = reader.IsDBNull(7) ? null : reader.GetString(7),
            Gender = (Gender)reader.GetInt16(8),
            CreateDate = reader.GetDateTime(9),
            UpdateDate = reader.GetDateTime(10),
            CategoryId = reader.GetInt16(11),
            Status = (ProductStatus)reader.GetInt16(12),
            ImageUrl = reader.IsDBNull(13) ? null : reader.GetString(13),
            MinPrice = reader.IsDBNull(14) ? null : reader.GetDecimal(14)

        };
    }
    private ProductItem MapProductItem(NpgsqlDataReader reader)
    {
        return new ProductItem
        {
            Id = reader.GetInt32(0),
            ProductId = reader.GetInt32(1),
            ProductCode = reader.IsDBNull(2) ? null : reader.GetString(2),
            OriginalPrice = reader.GetDecimal(3),
            SalePrice = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
            CreateDate = reader.GetDateTime(5),
            UpdateDate = reader.GetDateTime(6),
            Variants = reader.IsDBNull(7) ? null : JsonSerializer.Deserialize<Dictionary<string, string>>(reader.GetString(7)),
            ImageId = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
            Stock = new Stock
            {
                Id = reader.GetInt32(9),
                StockQuantity = reader.GetInt16(10),
                SelledItemsNbr = reader.GetInt16(11),
                ReservedItemsNbr = reader.GetInt16(12),
                ItemLength = reader.IsDBNull(13) ? null : (float?)reader.GetDouble(13),
                ItemWidth = reader.IsDBNull(14) ? null : (float?)reader.GetDouble(14),
                ItemHeight = reader.IsDBNull(15) ? null : (float?)reader.GetDouble(15),
                ItemWeight = reader.IsDBNull(16) ? null : (float?)reader.GetDouble(16),
                DiscountRate = reader.IsDBNull(17) ? null : (float?)reader.GetDouble(17),
                ExpirationDate = reader.IsDBNull(18) ? null : reader.GetDateTime(18),
                LastRestockDate = reader.IsDBNull(19) ? null : reader.GetDateTime(19),
                Status = (StockStatus)reader.GetInt16(20)
            }
        };
    }

    #endregion


    #region GetData

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"
                SELECT 
                    p.id, p.shop_id, p.serial_number, p.name, p.brand, p.about, 
                    p.ingredients, p.how_to_use, p.gender, p.create_date, p.update_date,
                    p.category_id, p.status,
                    pi.url as image_url
                FROM product p
                LEFT JOIN product_image pi ON pi.product_id = p.id AND pi.is_primary = true
                WHERE p.id = @productId";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@productId", productId);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return MapProduct(reader);
        }

        return null;
    }

    public async Task<IEnumerable<Product>> GetProductsByShopAsync(int shopId)
    {
        var products = new List<Product>();

        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"
                SELECT 
                    p.id, p.shop_id, p.serial_number, p.name, p.brand, p.about, 
                    p.ingredients, p.how_to_use, p.gender, p.create_date, p.update_date,
                    p.category_id, p.status,
                    pi.url as image_url
                FROM product p
                LEFT JOIN product_image pi ON pi.product_id = p.id AND pi.is_primary = true
                WHERE p.shop_id = @shopId";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@shopId", shopId);

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            products.Add(MapProduct(reader));
        }

        return products;
    }

    public async Task<ProductItem?> GetProductItemByIdAsync(int productItemId)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"
                SELECT 
                    pi.id, pi.product_id, pi.product_code, pi.original_price, pi.sale_price,
                    pi.create_date, pi.update_date, pi.item_variants, pi.image_id,
                    s.id as stock_id, s.stock_quantity, s.selled_items_nbr, s.reserved_items_nbr,
                    s.item_length, s.item_width, s.item_height, s.item_weight,
                    s.discount_rate, s.expiration_date, s.last_restock_date, s.status
                FROM product_item pi
                JOIN stock s ON pi.stock_id = s.id
                WHERE pi.id = @itemId";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@itemId", productItemId);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return MapProductItem(reader);
        }

        return null;
    }
    public async Task<IEnumerable<ProductItem>> GetProductItemsByProductIdAsync(int productId)
    {
        var items = new List<ProductItem>();

        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"
                SELECT 
                    pi.id, pi.product_id, pi.product_code, pi.original_price, pi.sale_price,
                    pi.create_date, pi.update_date, pi.item_variants, pi.image_id,
                    s.id as stock_id, s.stock_quantity, s.selled_items_nbr, s.reserved_items_nbr,
                    s.item_length, s.item_width, s.item_height, s.item_weight,
                    s.discount_rate, s.expiration_date, s.last_restock_date, s.status
                FROM product_item pi
                JOIN stock s ON pi.stock_id = s.id
                WHERE pi.product_id = @productId";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@productId", productId);

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            items.Add(MapProductItem(reader));
        }

        return items;
    }
    public async Task<ProductImage?> GetProductImageByIdAsync(int imageId)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();
        const string query = @"
                SELECT 
                    id, product_id, title, alternate_text, url, is_primary
                FROM product_image
                WHERE id = @imageId";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@imageId", imageId);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new ProductImage
            {
                Id = reader.GetInt32(0),
                ProductId = reader.GetInt32(1),
                Title = reader.IsDBNull(2) ? null : reader.GetString(2),
                AlternateText = reader.IsDBNull(3) ? null : reader.GetString(3),
                Url = reader.GetString(4),
                IsPrimary = reader.GetBoolean(5)
            };
        }
        return null;
    }

    #endregion

    #region AddData
    public async Task<int> CreateProductAsync(Product product)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"
                INSERT INTO product (
                    shop_id, serial_number, name, brand, about, ingredients, 
                    how_to_use, gender, category_id, status
                ) VALUES (
                    @shopId, @serialNumber, @name, @brand, @about, @ingredients,
                    @howToUse, @gender, @categoryId, @status
                ) RETURNING id";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@shopId", product.ShopId);
        command.Parameters.AddWithValue("@serialNumber", (object)product.SerialNumber ?? DBNull.Value);
        command.Parameters.AddWithValue("@name", (object)product.Name ?? DBNull.Value);
        command.Parameters.AddWithValue("@brand", (object)product.Brand ?? DBNull.Value);
        command.Parameters.AddWithValue("@about", (object)product.About ?? DBNull.Value);
        command.Parameters.AddWithValue("@ingredients", (object)product.Ingredients ?? DBNull.Value);
        command.Parameters.AddWithValue("@howToUse", (object)product.HowToUse ?? DBNull.Value);
        command.Parameters.AddWithValue("@gender", (int)product.Gender);
        command.Parameters.AddWithValue("@categoryId", product.CategoryId);
        command.Parameters.AddWithValue("@status", (int)product.Status);

        return (int)await command.ExecuteScalarAsync();
    }


    public async Task<int> AddProductImageAsync(ProductImage image)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"
                INSERT INTO product_image (
                    product_id, url, title, alternate_text, is_primary
                ) VALUES (
                    @productId, @url, @title, @alternateText, @isPrimary
                ) RETURNING id";

        using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("@productId", image.ProductId);
        command.Parameters.AddWithValue("@url", image.Url);
        command.Parameters.AddWithValue("@title", (object)image.Title ?? DBNull.Value);
        command.Parameters.AddWithValue("@alternateText", (object)image.AlternateText ?? DBNull.Value);
        command.Parameters.AddWithValue("@isPrimary", image.IsPrimary);

        return (int)await command.ExecuteScalarAsync();
    }


    public async Task<int> AddProductItemAsync(ProductItem productItem)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var stockId = await CreateStockAsync(connection, transaction, productItem.Stock);

            const string query = @"
                    INSERT INTO product_item (
                        product_id, product_code, original_price, sale_price,
                        item_variants, image_id, stock_id
                    ) VALUES (
                        @productId, @productCode, @originalPrice, @salePrice,
                        @itemVariants::json, @imageId, @stockId
                    ) RETURNING id";

            using var command = new NpgsqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@productId", productItem.ProductId);
            command.Parameters.AddWithValue("@productCode", (object)productItem.ProductCode ?? DBNull.Value);
            command.Parameters.AddWithValue("@originalPrice", productItem.OriginalPrice);
            command.Parameters.AddWithValue("@salePrice", (object)productItem.SalePrice ?? DBNull.Value);
            command.Parameters.AddWithValue("@itemVariants", productItem.Variants != null ? JsonSerializer.Serialize(productItem.Variants) : DBNull.Value);
            command.Parameters.AddWithValue("@imageId", (object)productItem.ImageId ?? DBNull.Value);
            command.Parameters.AddWithValue("@stockId", stockId);

            var itemId = (int)await command.ExecuteScalarAsync();

            await transaction.CommitAsync();
            return itemId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    private async Task<int> CreateStockAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, Stock stock)
    {
        const string query = @"
                INSERT INTO stock (
                    stock_quantity, selled_items_nbr, reserved_items_nbr,
                    item_length, item_width, item_height, item_weight,
                    discount_rate, expiration_date, last_restock_date, status
                ) VALUES (
                    @stockQuantity, @selledItemsNbr, @reservedItemsNbr,
                    @itemLength, @itemWidth, @itemHeight, @itemWeight,
                    @discountRate, @expirationDate, @lastRestockDate, @status
                ) RETURNING id";

        using var command = new NpgsqlCommand(query, connection, transaction);
        command.Parameters.AddWithValue("@stockQuantity", stock.StockQuantity);
        command.Parameters.AddWithValue("@selledItemsNbr", stock.SelledItemsNbr);
        command.Parameters.AddWithValue("@reservedItemsNbr", stock.ReservedItemsNbr);
        command.Parameters.AddWithValue("@itemLength", (object)stock.ItemLength ?? DBNull.Value);
        command.Parameters.AddWithValue("@itemWidth", (object)stock.ItemWidth ?? DBNull.Value);
        command.Parameters.AddWithValue("@itemHeight", (object)stock.ItemHeight ?? DBNull.Value);
        command.Parameters.AddWithValue("@itemWeight", (object)stock.ItemWeight ?? DBNull.Value);
        command.Parameters.AddWithValue("@discountRate", (object)stock.DiscountRate ?? DBNull.Value);
        command.Parameters.AddWithValue("@expirationDate", (object)stock.ExpirationDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@lastRestockDate", (object)stock.LastRestockDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@status", (int)stock.Status);

        return (int)await command.ExecuteScalarAsync();
    }

    public async Task<int> AddProductItemImageAsync(int productItemId, ProductImage image)
    {

        var imageId = await AddProductImageAsync(image);

        // Then update the product item
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"
                UPDATE product_item SET
                    image_id = @imageId,
                    update_date = CURRENT_TIMESTAMP
                WHERE id = @itemId";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@itemId", productItemId);
        command.Parameters.AddWithValue("@imageId", imageId);

        await command.ExecuteNonQueryAsync();

        return imageId;
    }

    #endregion

    #region UpdateData
    public async Task<bool> UpdateProductAsync(Product product)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"
                UPDATE product SET
                    serial_number = @serialNumber,
                    name = @name,
                    brand = @brand,
                    about = @about,
                    ingredients = @ingredients,
                    how_to_use = @howToUse,
                    gender = @gender,
                    category_id = @categoryId,
                    status = @status,
                    update_date = CURRENT_TIMESTAMP
                WHERE id = @id";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", product.Id);
        command.Parameters.AddWithValue("@serialNumber", (object)product.SerialNumber ?? DBNull.Value);
        command.Parameters.AddWithValue("@name", (object)product.Name ?? DBNull.Value);
        command.Parameters.AddWithValue("@brand", (object)product.Brand ?? DBNull.Value);
        command.Parameters.AddWithValue("@about", (object)product.About ?? DBNull.Value);
        command.Parameters.AddWithValue("@ingredients", (object)product.Ingredients ?? DBNull.Value);
        command.Parameters.AddWithValue("@howToUse", (object)product.HowToUse ?? DBNull.Value);
        command.Parameters.AddWithValue("@gender", (int)product.Gender);
        command.Parameters.AddWithValue("@categoryId", product.CategoryId);
        command.Parameters.AddWithValue("@status", (int)product.Status);

        return await command.ExecuteNonQueryAsync() > 0;
    }
    public async Task<bool> UpdateProductItemAsync(ProductItem productItem)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            // First update the stock record
            await UpdateStockAsync(connection, transaction, productItem.Stock);

            // Then update the product item
            const string query = @"
                    UPDATE product_item SET
                        product_code = @productCode,
                        original_price = @originalPrice,
                        sale_price = @salePrice,
                        item_variants = @itemVariants::json,
                        image_id = @imageId,
                        update_date = CURRENT_TIMESTAMP
                    WHERE id = @id";

            using var command = new NpgsqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@id", productItem.Id);
            command.Parameters.AddWithValue("@productCode", (object)productItem.ProductCode ?? DBNull.Value);
            command.Parameters.AddWithValue("@originalPrice", productItem.OriginalPrice);
            command.Parameters.AddWithValue("@salePrice", (object)productItem.SalePrice ?? DBNull.Value);
            command.Parameters.AddWithValue("@itemVariants", productItem.Variants != null ? JsonSerializer.Serialize(productItem.Variants) : DBNull.Value);
            command.Parameters.AddWithValue("@imageId", (object)productItem.ImageId ?? DBNull.Value);

            var rowsAffected = await command.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            return rowsAffected > 0;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task UpdateStockAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, Stock stock)
    {
        const string query = @"
                UPDATE stock SET
                    stock_quantity = @stockQuantity,
                    selled_items_nbr = @selledItemsNbr,
                    reserved_items_nbr = @reservedItemsNbr,
                    item_length = @itemLength,
                    item_width = @itemWidth,
                    item_height = @itemHeight,
                    item_weight = @itemWeight,
                    discount_rate = @discountRate,
                    expiration_date = @expirationDate,
                    last_restock_date = @lastRestockDate,
                    status = @status
                WHERE id = @id";

        using var command = new NpgsqlCommand(query, connection, transaction);
        command.Parameters.AddWithValue("@id", stock.Id);
        command.Parameters.AddWithValue("@stockQuantity", stock.StockQuantity);
        command.Parameters.AddWithValue("@selledItemsNbr", stock.SelledItemsNbr);
        command.Parameters.AddWithValue("@reservedItemsNbr", stock.ReservedItemsNbr);
        command.Parameters.AddWithValue("@itemLength", (object)stock.ItemLength ?? DBNull.Value);
        command.Parameters.AddWithValue("@itemWidth", (object)stock.ItemWidth ?? DBNull.Value);
        command.Parameters.AddWithValue("@itemHeight", (object)stock.ItemHeight ?? DBNull.Value);
        command.Parameters.AddWithValue("@itemWeight", (object)stock.ItemWeight ?? DBNull.Value);
        command.Parameters.AddWithValue("@discountRate", (object)stock.DiscountRate ?? DBNull.Value);
        command.Parameters.AddWithValue("@expirationDate", (object)stock.ExpirationDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@lastRestockDate", (object)stock.LastRestockDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@status", (int)stock.Status);

        await command.ExecuteNonQueryAsync();
    }

    #endregion

    #region DeleteData

    public async Task<bool> DeleteProductItemAsync(int itemId)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            // First get the stock ID
            var stockId = await GetStockIdByItemId(connection, transaction, itemId);

            if (stockId == 0)
            {
                await transaction.RollbackAsync();
                return false;
            }

            // Delete the product item
            const string deleteItemQuery = "DELETE FROM product_item WHERE id = @id";
            using var deleteItemCommand = new NpgsqlCommand(deleteItemQuery, connection, transaction);
            deleteItemCommand.Parameters.AddWithValue("@id", itemId);
            await deleteItemCommand.ExecuteNonQueryAsync();

            // Delete the associated stock
            const string deleteStockQuery = "DELETE FROM stock WHERE id = @stockId";
            using var deleteStockCommand = new NpgsqlCommand(deleteStockQuery, connection, transaction);
            deleteStockCommand.Parameters.AddWithValue("@stockId", stockId);
            await deleteStockCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<int> GetStockIdByItemId(NpgsqlConnection connection, NpgsqlTransaction transaction, int itemId)
    {
        const string query = "SELECT stock_id FROM product_item WHERE id = @itemId";

        using var command = new NpgsqlCommand(query, connection, transaction);
        command.Parameters.AddWithValue("@itemId", itemId);

        var result = await command.ExecuteScalarAsync();
        return result != null ? (int)result : 0;
    }


    public async Task<bool> DeleteProductImageAsync(int imageId)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();
        const string query = "DELETE FROM product_image WHERE id = @imageId";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@imageId", imageId);
        return await command.ExecuteNonQueryAsync() > 0;
    }

    #endregion


    #region SearchData

    /* I applied here the Faceted Search, see the below references:
         * https://stackoverflow.com/questions/5321595/what-is-faceted-search
         * Also use ChatGPT for explanation
         */


    public async Task<IEnumerable<Product>> SearchProductsAsync(ProductSearchCriteria criteria)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        string productsQuery = BuildQueries(criteria);

        var command = CreateCommand(connection, productsQuery, criteria);

        var products = new List<Product>();

        using var reader = await command.ExecuteReaderAsync();


        while (await reader.ReadAsync())
        {
            products.Add(MapProduct(reader));
        }

        return products;

    }

    private string BuildQueries(ProductSearchCriteria criteria)
    {
        // Product entity with added column for the lowest price in the product items collection

        var productsQuery = new StringBuilder(@"
            SELECT DISTINCT (p.id), p.shop_id, p.serial_number, p.name, p.brand, p.about, 
                   p.ingredients, p.how_to_use, p.gender, p.create_date, p.update_date,
                   p.category_id, p.status_id,
                   pi.url as image_url,
                   (SELECT MIN(pit.original_price) 
                    FROM product_item pit 
                    WHERE pit.product_id = p.id) as min_price
            FROM product p
            LEFT JOIN product_image pi ON pi.product_id = p.id AND pi.is_primary = true
            LEFT JOIN product_item pit on pit.product_id = p.id
            WHERE p.status_id = (SELECT id FROM product_status WHERE name = 'Published')");


        // Add filters


        if (criteria.CategoryIds?.Count > 0)
        {
            var categories = " AND p.category_id = ANY(@categoryIds)";
            productsQuery.Append(categories);
        }

        if (criteria.Genders.HasValue)
        {
            var gender = " AND p.gender = @gender";
            productsQuery.Append(gender);
        }

        if (criteria.MinPrice.HasValue)
        {
            var minPrice = " AND pit.original_price >= @minPrice";
            productsQuery.Append(minPrice);
        }

        if (criteria.MaxPrice.HasValue)
        {
            var maxPrice = " AND pit.original_price <= @maxPrice";
            productsQuery.Append(maxPrice);
        }

        if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
        {
            var searchTerm = @" AND (p.name ILIKE @searchTerm OR p.about ILIKE @searchTerm OR 
                                         p.brand ILIKE @searchTerm OR p.ingredients ILIKE @searchTerm OR
                                         p.how_to_use ILIKE @searchTerm)";
            productsQuery.Append(searchTerm);
        }


        return productsQuery.ToString() + ";";
    }



    private NpgsqlCommand CreateCommand(NpgsqlConnection connection, string query, ProductSearchCriteria criteria)
    {
        var command = new NpgsqlCommand(query, connection);

        if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
        {
            command.Parameters.AddWithValue("@searchTerm", $"%{criteria.SearchTerm}%");
        }

        if (criteria.CategoryIds?.Count > 0)
        {
            command.Parameters.AddWithValue("@categoryIds", criteria.CategoryIds.ToArray());
        }

        if (criteria.Genders.HasValue)
        {
            command.Parameters.AddWithValue("@gender", criteria.Genders.ToString());
        }

        if (criteria.MinPrice.HasValue)
        {
            command.Parameters.AddWithValue("@minPrice", criteria.MinPrice.Value);
        }

        if (criteria.MaxPrice.HasValue)
        {
            command.Parameters.AddWithValue("@maxPrice", criteria.MaxPrice.Value);
        }


        return command;
    }

    #endregion

}
