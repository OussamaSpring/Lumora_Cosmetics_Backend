using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities.ProductRelated;

namespace Application.Services;

public class SearchService : ISearchService
{
    private readonly IProductRepository _productRepository;
    public SearchService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> Search(ProductSearchCriteria criteria)
    {
        return await _productRepository.SearchProductsAsync(criteria);
    }
}
