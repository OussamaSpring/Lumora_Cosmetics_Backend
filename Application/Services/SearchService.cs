using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities.ProductRelated;

namespace Application.Services;

public class SearchService : ISearchService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public SearchService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Product>> Search(ProductSearchCriteria criteria)
    {
        if (criteria.CategoryIds != null && criteria.CategoryIds.Any()) // category filter in user side is done by categories of second level only
        {
            var childCategoryIds = await _categoryRepository.GetChildCategoriesIDsAsync(criteria.CategoryIds);
            criteria.CategoryIds = childCategoryIds?.ToList();
        }

        return await _productRepository.SearchProductsAsync(criteria);
    }
}
