using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities.ShopRelated;

namespace Application.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;

        public ShopService(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        public async Task CreateOrUpdateShopAsync(UpdateShopDto dto, Guid vendorId)
        {
            // Check if shop exists for the vendor
            var existingShop = await _shopRepository.GetShopByVendorIdAsync(vendorId);

            if (existingShop != null)
            {
                // Update existing shop
                existingShop.Name = dto.Name;
                existingShop.Description = dto.Description;
                existingShop.LogoUrl = dto.LogoUrl;
                existingShop.MapAddress = dto.MapAddress;
                existingShop.UpdateDate = DateTime.UtcNow;

                await _shopRepository.UpdateShopAsync(existingShop);
            }
            else
            {
                // Create a new shop
                var newShop = new Shop
                {
                    VendorId = vendorId,
                    Name = dto.Name,
                    Description = dto.Description,
                    LogoUrl = dto.LogoUrl,
                    MapAddress = dto.MapAddress,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                await _shopRepository.CreateShopAsync(newShop);
            }
        }
    }
}