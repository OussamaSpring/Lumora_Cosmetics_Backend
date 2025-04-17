using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPut("/supdate")]
        public async Task<IActionResult> CreateOrUpdateShop([FromBody] UpdateShopDto dto)
        {
            // var vendorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "vendorId");
            // if (vendorIdClaim == null)
            //     return Unauthorized(new { message = "VendorId not found in token" });

            // if (!Guid.TryParse(vendorIdClaim.Value, out Guid vendorId))
            //     return Unauthorized(new { message = "Invalid VendorId format" });

            await _shopService.CreateOrUpdateShopAsync(dto,);
            return Ok(new { message = "Shop created or updated successfully" });
        }

        private IActionResult Unauthorized(object value)
        {
            throw new NotImplementedException();
        }
    }
}