using Domain.Entities.ProductRelated;
using Domain.Shared;

namespace Application.Interfaces.Services;

public interface ISearchService
{
    Task<Result<IEnumerable<Product>?>> Search(ProductSearchCriteria criteria);
}
