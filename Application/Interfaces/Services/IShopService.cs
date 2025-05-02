using Application.DTOs;
using Domain.Entities.ShopRelated;

namespace Application.Interfaces.Services
{
    public interface IShopService
    {
        Task<Shop> CreateShopAsync(UpdateShopDto dto, Guid vendorId);
        Task UpdateShopAsync(UpdateShopDto dto, Guid vendorId);
    }
}
