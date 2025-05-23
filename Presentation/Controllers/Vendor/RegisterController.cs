using Application.DTOs.Category;
using Application.Interfaces.Services;
using Domain.Enums.Account;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Vendor;

[Route("api/vendor/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly IUserAuthentication _userAuthentication;

    public RegisterController(IUserAuthentication userAuthentication)
    {
        _userAuthentication = userAuthentication;
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { ErrorMessage = "Register request input" });

        var result = await _userAuthentication.Register(registerRequest, UserRole.Vendor);
        return result.IsSuccess ? Ok(new { AccessToken = result.Value })
            : BadRequest(new { ErrorMessage = result.Error.description });
    }
}
