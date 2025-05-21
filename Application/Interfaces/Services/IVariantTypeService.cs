using Application.DTOs.VarianteType;
using Application.DTOs.VariantType;
using Domain.Entities.ProductRelated;
using Domain.Shared;

namespace Application.Interfaces.Services
{
    public interface IVariantTypeService
    {
        Task<Result<VariantType?>> CreateVariantTypeAsync(int categoryId, CreateVariantTypeRequest request);
        Task<Result> DeleteVariantType(int id);
        Task<Result<IEnumerable<VariantType>>> GetVariantTypesForCategoryAsync(int categoryId);
        Task<Result<VariantType?>> GetVariantTypeByIdAsync(int id);
        Task<Result<VariantType?>> UpdateVariantAsync(int variantTypeId, UpdateVariantTypeRequest request);
    }
}