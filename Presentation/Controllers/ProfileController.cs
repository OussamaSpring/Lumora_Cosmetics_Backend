using Application.DTOs;
using Application.DTOs.Profile;
using Application.Interfaces.Services;
using Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetProfileAsync([FromRoute] Guid id)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.GetUser(id);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.description);
        }

        return BadRequest(Result.
            Failure(new Error("Register request input", "Invalid")));
    }

    [HttpPut("personal-information/{id:guid}")]
    public async Task<ActionResult> UpdatePersonalInformationAsync(
        [FromRoute] Guid id,
        UpdatePersonalInformationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(Result
                .Failure(new Error("Register request input", "Invalid")));
        }

        var result = await _userService.UpdateProfileInformationAsync(id, request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPut("update-credentials/{id:guid}")]
    public async Task<ActionResult> UpdateCredentialsAsync(
    [FromRoute] Guid id,
    UpdateCredentialsRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(Result
                .Failure(new Error("Register request input", "Invalid")));
        }

        var result = await _userService.UpdateCredentialsAsync(id, request);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpPut("update-photo/{id:guid}")]
    public async Task<IActionResult> UpdatePhoto([FromRoute] Guid id, IFormFile file)
    {
        var result = await _userService.UpdateUserPhoto(id, file);
        return result.IsSuccess ? Ok(result.Value): BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(Result
                .Failure(new Error("Register request input", "Invalid")));
        }


        var result = await _userService.DeleteUserAsync(id);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}