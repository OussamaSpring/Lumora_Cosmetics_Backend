using Domain.Entities.ProductRelated;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}
