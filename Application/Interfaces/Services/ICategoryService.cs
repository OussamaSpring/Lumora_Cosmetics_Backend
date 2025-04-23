using Application.DTOs.Category;
using Domain.Entities.ProductRelated;
using Domain.Shared;

namespace Application.Interfaces.Services;

public interface ICategoryService
{
    Task<Result<Category?>> GetCategoryByIdAsync(int id);
    Task<Result<IEnumerable<Category>>> GetAllCategoryAsync();
    Task<Result<Category>> CreateCategoryAsync(CreateCategoryRequest request);
    Task<Result<Category?>> UpdateCategoryAsync(int categoryId, UpdateCategoryRequest category);
    Task<Result<Category?>> DeleteCategoryAsync(int categoryId);
}
