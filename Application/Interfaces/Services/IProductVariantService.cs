using Application.DTOs.Product;
using Domain.Shared;

namespace Application.Interfaces.Services
{
    public interface IProductVariantService
    {
        Task<Result<ProductItemDto>> AddVariant(int productId, CreateVariantDto dto);
        Task<Result<ProductItemDto>> GetVariant(int variantId);
        Task<Result<IEnumerable<ProductItemDto>>> GetVariantsByProduct(int productId);
        Task<Result<ProductItemDto>> UpdateVariant(int variantId, UpdateVariantDto dto);
        Task<Result<bool>> DeleteVariant(int variantId);
    }
}
