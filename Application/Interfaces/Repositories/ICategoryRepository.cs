using Domain.Entities.ProductRelated;
using Domain.Shared;


namespace Application.Interfaces.Repositories;
public interface ICategoryRepository
{
    Task<Category?> GetCategoryByIdAsync(int categoryId);
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<IEnumerable<VariantType>> GetVariantTypesForCategoryAsync(int categoryId); // this is the most important
    Task<int> CreateAsync(Category category);
    Task<Category?> DeleteAsync(int categoryId);
    Task UpdateCategoryAsync(int categoryId, Category category);

    /*
     * The methods below are commented out because they are not used in the current implementation.
     * They are realted to the admin functionalities.


    Task<int> CreateCategoryAsync(Category category);
    Task<bool> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(int categoryId);
    Task<short> AddVariantTypeAsync(VariantType variantType);
    Task<bool> UpdateVariantTypeAsync(VariantType variantType);
    Task<bool> DeleteVariantTypeAsync(int variantTypeId);

    */
}