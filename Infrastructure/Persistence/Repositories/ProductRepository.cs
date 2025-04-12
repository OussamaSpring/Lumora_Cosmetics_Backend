using Application.Interfaces;
using Domain.Entities.ProductRelated;
using Domain.Enums.enProduct;
using Npgsql;

namespace Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        throw new NotImplementedException();
    }





    public async Task<Product?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    // --- Helper Methods ---

    private void AddParameters(NpgsqlCommand cmd, Product product, bool includeId)
    {
        throw new NotImplementedException();
    }

    private Product MapProduct(NpgsqlDataReader reader)
    {
        throw new NotImplementedException();
    }

    public Task CreateWithItemsAsync(Product product, List<ProductItem> productItems)
    {
        throw new NotImplementedException();
    }

    Task IProductRepository.UpdateAsync(Product product)
    {
        return UpdateAsync(product);
    }
}
