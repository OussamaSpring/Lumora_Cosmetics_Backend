using System.Security.Claims;
using Application.DTOs.Address;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;
    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet("{addressId:int}")]
    public async Task<IActionResult> GetAddress(int addressId)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid model state");

        var result = await _addressService.GetAddressById(addressId);
        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<IActionResult> AddAddress(
        CreateAddressRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid model state");

        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(id, out Guid userId))
            return BadRequest("id fault");

        var result = await _addressService.CreateAddress(userId, request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.description);
    }

    [HttpDelete("{addressId:int}")]
    public async Task<IActionResult> DeleteAddress([FromRoute] int addressId)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid model state");

        var result = await _addressService.DeleteAddress(addressId);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.description);
    }

    [Authorize(Roles = "Customer")]
    [HttpGet]
    public async Task<IActionResult> GetAllAddresses()
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid model state");

        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(id, out Guid userId))
            return BadRequest("id fault");

        var result = await _addressService.GetAllAddresses(userId);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.description);
    }

    [HttpPut("{addressId:int}")]
    public async Task<IActionResult> UpdateAddress(
        [FromRoute] int addressId,
        UpdateAddressRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid model state");

        var result = await _addressService.UpdateAddress(addressId, request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.description);
    }
}
