using Application.DTOs.Product;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    }
}
