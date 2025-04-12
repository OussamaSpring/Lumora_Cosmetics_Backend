using Domain.Entities.ProductRelated;


namespace Application.Interfaces.Repositories;
public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<IEnumerable<VariantType>> GetVariantTypesForCategoryAsync(int categoryId); // this is the most important
    Task<int> CreateCategoryAsync(Category category);
    Task<bool> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(int categoryId);
    Task<short> AddVariantTypeAsync(VariantType variantType);
    Task<bool> UpdateVariantTypeAsync(VariantType variantType);
    Task<bool> DeleteVariantTypeAsync(int variantTypeId);
}