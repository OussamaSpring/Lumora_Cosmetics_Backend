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

        public async Task<Shop> CreateShopAsync(UpdateShopDto dto, Guid vendorId)
        {
            var existingShop = await _shopRepository.GetShopByVendorIdAsync(vendorId);
            if (existingShop != null)
            {
                throw new InvalidOperationException("A shop already exists for this vendor.");
            }

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

            int id = await _shopRepository.CreateShopAsync(newShop);
            newShop.Id = id;
            return (newshop);
        }

        public async Task UpdateShopAsync(UpdateShopDto dto, Guid vendorId)
        {
            var existingShop = await _shopRepository.GetShopByVendorIdAsync(vendorId);
            if (existingShop == null)
            {
                throw new KeyNotFoundException("No shop found for this vendor.");
            }

            existingShop.Name = dto.Name;
            existingShop.Description = dto.Description;
            existingShop.LogoUrl = dto.LogoUrl;
            existingShop.MapAddress = dto.MapAddress;
            existingShop.UpdateDate = DateTime.UtcNow;

            await _shopRepository.UpdateShopAsync(existingShop);
        }
    }
}
