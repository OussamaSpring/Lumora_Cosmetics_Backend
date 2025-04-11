using Application.DTOs;
using Application.Interfaces.Services;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.User;

[Route("api/customer/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly IUserAuthentication _userAuthentication;

    public RegisterController(IUserAuthentication userAuthentication)
    {
        _userAuthentication = userAuthentication;
    }

    [HttpPost]
    public ActionResult<Result<string?>> Register(RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(Result<string>.Failure(new Error("Register request input", "Invalid")));
        }

        var result = _userAuthentication.Register(registerRequest);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
