using Application.DTOs;
using Application.Interfaces.Services;
using Domain.Entities.ShopRelated;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpPost("{vendorId:guid}")]
        public async Task<IActionResult> CreateShop(Guid vendorId, [FromBody] UpdateShopDto dto)
        {
            Shop shop = await _shopService.CreateShopAsync(dto, vendorId);
            return Ok(new { Shop = shop, message = "Shop created successfully" });
        }

        [HttpPut("{vendorId:guid}")]
        public async Task<IActionResult> UpdateShop(Guid vendorId, [FromBody] UpdateShopDto dto)
        {
            await _shopService.UpdateShopAsync(dto, vendorId);
            return Ok(new { message = "Shop updated successfully" });
        }
    }
}
