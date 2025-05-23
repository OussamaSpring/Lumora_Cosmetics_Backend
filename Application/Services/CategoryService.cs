using Application.DTOs.Category;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities.ProductRelated;
using Domain.Shared;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Category>> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        int id = await _categoryRepository.CreateAsync(category);
        category.Id = id;

        return Result<Category>.Success(category);
    }

    public async Task<Result<Category?>> DeleteCategoryAsync(int categoryId)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
        if (category is null)
            return Result<Category>
                .Failure(new Error("CategoryService.DeleteCategoryAsync", "category does not exist"));

        // change
        return Result<Category?>.Success(await _categoryRepository.DeleteAsync(categoryId));
    }

    public async Task<Result<IEnumerable<Category>>> GetAllCategoryAsync()
    {
        return Result<IEnumerable<Category>>
            .Success(await _categoryRepository.GetAllCategoriesAsync());
    }

    public async Task<Result<Category?>> UpdateCategoryAsync(
        int categoryId, 
        UpdateCategoryRequest request)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
        if (category is null)
            return Result<Category?>
                .Failure(new Error("udpateCategoryAsync", "category does not exist"));

        category.Name = request.Name;
        category.Description = request.Description;
        await _categoryRepository.UpdateCategoryAsync(categoryId, category);
        return Result<Category?>.Success(category);
    }

    public async Task<Result<Category?>> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category is null)
        {
            return Result<Category?>.
                Failure(new Error(nameof(GetCategoryByIdAsync), "category does not exists"));
        }

        return Result<Category?>.Success(category);
    }
}
