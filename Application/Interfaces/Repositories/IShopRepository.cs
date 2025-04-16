using Domain.Entities.ShopRelated;

namespace Application.Interfaces.Repositories
{
    public interface IShopRepository
    {
        Task<Shop?> GetShopByVendorIdAsync(Guid vendorId);
        Task CreateShopAsync(Shop shop);
        Task UpdateShopAsync(Shop shop);
    
    }
}