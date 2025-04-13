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

    public Task<Product> CreateAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task CreateWithItemsAsync(Product product, List<ProductItem> productItems)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetByIdWithDetailsAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Product product)
    {
        throw new NotImplementedException();
    }
}
