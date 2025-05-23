using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private IUserAuthentication _userAuthentication;
    public LoginController(IUserAuthentication userAuthentication)
    {
        _userAuthentication = userAuthentication;
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest("input is not valid");

        var result = await _userAuthentication.Login(loginRequest);
        return result.IsSuccess ? Ok(new { AccessToken = result.Value })
            : BadRequest(new { ErrorMessage = result.Error.description });
    }
}
