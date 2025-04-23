using Domain.Entities.ProductRelated;

namespace Application.Interfaces.Repositories;

public interface IVarianteTypeRepository
{
    Task<int> AddVariantTypeAsync(VariantType variant);
    Task DeleteVariantTypeAsync(int id);
    Task<VariantType?> GetVariantTypeByIdAsync(int id);
    Task<IEnumerable<VariantType>> GetVariantTypeForCategory(int categoryId);
    Task UpdateVariantTypeAsync(int variantTypeId, VariantType variant);
}