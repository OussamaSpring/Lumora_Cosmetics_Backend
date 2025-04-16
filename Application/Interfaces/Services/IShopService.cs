using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface IShopService
    {
        Task CreateOrUpdateShopAsync(UpdateShopDto dto, Guid vendorId);
    }
}