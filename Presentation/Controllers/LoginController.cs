using Application.DTOs;
using Application.Interfaces.Services;
using Domain.Shared;
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

    //[HttpPost("login/")]
    [HttpPost]
    public ActionResult<Result<string>> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(Result<string>.Failure(new Error("Body Request", "input is not valid")));
        }

        var result = _userAuthentication.Login(loginRequest);
        return result.IsSuccess ? Ok(new
        {
            Token = result.Value,
            Success = result.IsSuccess
        }) : Unauthorized();

    }
}
