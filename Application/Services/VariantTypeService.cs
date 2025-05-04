using Application.DTOs.VarianteType;
using Application.DTOs.VariantType;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities.ProductRelated;
using Domain.Shared;

namespace Application.Services;

public class VariantTypeService : IVariantTypeService
{
    private readonly IVariantTypeRepository _variantTypeRepository;
    private readonly ICategoryRepository _categoryRepository;

    public VariantTypeService(ICategoryRepository categoryRepository, IVariantTypeRepository varianteTypeRepository)
    {
        _categoryRepository = categoryRepository;
        _variantTypeRepository = varianteTypeRepository;
    }

    public async Task<Result<VariantType?>> CreateVariantTypeAsync(
        int categoryId,
        CreateVariantTypeRequest request)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
        if (category is null)
        {
            return Result<VariantType?>
                .Failure(new Error("createVariantType", "category does not exist"));
        }

        var variant = new VariantType
        {
            Name = request.Name,
            Description = request.Description,
            CategoryId = categoryId,
        };

        variant.Id = await _variantTypeRepository.AddVariantTypeAsync(variant);
        return Result<VariantType?>.Success(variant);
    }

    public async Task<Result> DeleteVariantType(int id)
    {
        if (await _variantTypeRepository.GetVariantTypeByIdAsync(id) is null)
        {
            return Result
                .Failure(new Error("delete variant", "variant does not exist"));
        }
        await _variantTypeRepository.DeleteVariantTypeAsync(id);
        return Result.Success();
    }

    public async Task<Result<VariantType?>> GetVariantTypeByIdAsync(int id)
    {
        var variant = await _variantTypeRepository.GetVariantTypeByIdAsync(id);
        if (variant is null)
            return Result<VariantType?>.Failure(new Error("ddsd", "dds"));

        return Result<VariantType?>.Success(variant);
    }

    public async Task<Result<VariantType?>> UpdateVariantAsync(
        int variantTypeId,
        UpdateVariantTypeRequest request)
    {
        var variant = await _variantTypeRepository.GetVariantTypeByIdAsync(variantTypeId);
        if (variant is null)
            return Result<VariantType?>
                .Failure(new Error("updateVariantType", $"does not exist variant with {variantTypeId}"));

        variant.Name = request.Name;
        variant.Description = request.Description;

        await _variantTypeRepository.UpdateVariantTypeAsync(variantTypeId, variant);
        return Result<VariantType?>.Success(variant);
    }

    public async Task<Result<IEnumerable<VariantType>>> GetVariantTypesForCategoryAsync(int categoryId)
    {
        return Result<IEnumerable<VariantType>>
            .Success(await _variantTypeRepository.GetVariantTypeForCategory(categoryId));
    }
}
