using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities.ProductRelated;
using Domain.Shared;

namespace Application.Services;

public class SearchService : ISearchService
{
    private readonly IProductRepository _productRepository;
    public SearchService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<IEnumerable<Product>?>> Search(ProductSearchCriteria criteria)
    {
        var products = await _productRepository.SearchProductsAsync(criteria);
        return Result<IEnumerable<Product>?>.Success(products);
    }
}
