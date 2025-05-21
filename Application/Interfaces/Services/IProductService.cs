using Application.DTOs.Product;
using Domain.Enums.Product;

namespace Application.Interfaces.Services
{
    public interface IProductService
    {
        // Product CRUD Operations
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        Task<ProductDto?> GetProductByIdAsync(int productId);
        Task<IEnumerable<ProductBriefDto>> GetProductsByShopAsync(int shopId);
        Task<bool> UpdateProductAsync(UpdateProductDto dto);
        Task<bool> DeleteProductAsync(int productId);

        // ProductItem Operations
        Task<ProductItemDto> CreateProductItemAsync(CreateVariantDto dto);
        Task<ProductItemDto?> GetProductItemByIdAsync(int productItemId);
        Task<IEnumerable<ProductItemDto>> GetProductItemsByProductIdAsync(int productId);
        Task<bool> UpdateProductItemAsync(int itemId,UpdateVariantDto dto);
        Task<bool> DeleteProductItemAsync(int productItemId);

        // Product Image Operations
        Task<ProductImageDto> AddProductImageAsync(AddImageDto dto);
        Task<ProductImageDto?> GetProductImageByIdAsync(int imageId);
        Task<bool> DeleteProductImageAsync(int imageId);
        Task<int> AddProductItemImageAsync(int productItemId, AddImageDto dto);

        // Status Management
        Task<bool> UpdateProductStatusAsync(int productId, ProductStatus status);
        Task<bool> UpdateStockStatusAsync(int productItemId, StockStatus status);
    }
}