using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface IShopService
    {
        Task CreateShopAsync(UpdateShopDto dto, Guid vendorId);
        Task UpdateShopAsync(UpdateShopDto dto, Guid vendorId);
    }
}
