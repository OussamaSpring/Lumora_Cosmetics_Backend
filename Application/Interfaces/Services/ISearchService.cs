using Domain.Entities.ProductRelated;

namespace Application.Interfaces.Services;

public interface ISearchService
{
    Task<IEnumerable<Product>> Search(ProductSearchCriteria criteria);
}
