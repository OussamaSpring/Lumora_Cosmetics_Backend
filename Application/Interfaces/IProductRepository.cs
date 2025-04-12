using Domain.Entities.ProductRelated;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        Task CreateWithItemsAsync(Product product, List<ProductItem> productItems);
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task UpdateAsync(Product product);
    }

}
