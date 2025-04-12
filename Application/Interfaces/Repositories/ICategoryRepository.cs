using Domain.Entities.ProductRelated;


namespace Application.Interfaces.Repositories;
public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<IEnumerable<VariantType>> GetVariantTypesForCategoryAsync(int categoryId); // this is the most important
    Task<int> CreateCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int categoryId);
    Task AddVariantTypeAsync(VariantType variantType);
    Task UpdateVariantTypeAsync(VariantType variantType);
    Task DeleteVariantTypeAsync(int variantTypeId);
}