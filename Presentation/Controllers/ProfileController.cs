using System.Security.Claims;
using Application.DTOs.Profile;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult> GetProfileAsync()
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userId, out Guid id))
            return BadRequest("Invalid JWT token");

        var result = await _userService.GetUser(id);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.description);
    }

    [HttpPut("personal-information/")]
    public async Task<ActionResult> UpdatePersonalInformationAsync(
        UpdatePersonalInformationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Register request input");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userId, out Guid id))
            return BadRequest("id fault");

        var result = await _userService.UpdateProfileInformationAsync(id, request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPut("update-credentials/")]
    public async Task<ActionResult> UpdateCredentialsAsync(
        UpdateCredentialsRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Register request input");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userId, out Guid id))
            return BadRequest("id fault");

        var result = await _userService.UpdateCredentialsAsync(id, request);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.description);
    }

    [HttpPut("update-photo/")]
    public async Task<IActionResult> UpdatePhoto(IFormFile file)
    {
        if (!ModelState.IsValid)
            return BadRequest("Register request input");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userId, out Guid id))
            return BadRequest("id fault");

        var result = await _userService.UpdateUserPhoto(id, file);
        return result.IsSuccess ? Ok(result.Value): BadRequest(result.Error.description);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser()
    {
        if (!ModelState.IsValid)
            return BadRequest("Register request input");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userId, out Guid id))
            return BadRequest("id fault");

        var result = await _userService.DeleteUserAsync(id);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.description);
    }
}