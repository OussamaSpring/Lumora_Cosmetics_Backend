using System.Reflection.Metadata.Ecma335;
using Application.DTOs.Address;
using Application.Interfaces.Services;
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
        {
            return BadRequest(ModelState);
        }
        var result = await _addressService.GetAddressById(addressId);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost("{userId:guid}")]
    public async Task<IActionResult> AddAddress(
        [FromRoute] Guid userId,
        CreateAddressRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _addressService.CreateAddress(userId, request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest($"{result.Error}");
    }

    [HttpDelete("{addressId:int}")]
    public async Task<IActionResult> DeleteAddress([FromRoute] int addressId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _addressService.DeleteAddress(addressId);
        return result.IsSuccess ? Ok() : BadRequest($"{result.Error}");
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetAllAddresses([FromRoute] Guid userId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _addressService.GetAllAddresses(userId);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPut("{addressId:int}")]
    public async Task<IActionResult> UpdateAddress(
        [FromRoute] int addressId,
        UpdateAddressRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _addressService.UpdateAddress(addressId, request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest($"{result.Error}");
    }

}
