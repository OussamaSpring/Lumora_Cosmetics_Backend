//using Application.DTOs;
//using Application.Interfaces.Services;
//using Domain.Shared;
//using Microsoft.AspNetCore.Mvc;

//namespace Presentation.Controllers.Vendor;

//[Route("api/vendor/profile")]
//[ApiController]
//public class ProfileController : ControllerBase
//{
//    private readonly IUserService _userService;

//    public ProfileController(IUserService userService)
//    {
//        _userService = userService;
//    }

//    [HttpGet("/{id:guid}")]
//    public async Task<ActionResult> GetProfileAsync([FromRoute] Guid id)
//    {
//        if (ModelState.IsValid)
//        {
//            var result = await _userService.GetUser(id);
//            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.description);
//        }

//        return BadRequest(Result.
//            Failure(new Error("Register request input", "Invalid")));
//    }

//    [HttpPut("/{id:guid}")]
//    public async Task<ActionResult> UpdateProfileAsync([FromRoute] Guid id, UpdateUserRequest updateUser)
//    {
//        if (!ModelState.IsValid)
//        {
//            return BadRequest(Result
//                .Failure(new Error("Register request input", "Invalid")));
//        }

//        var result = await _userService.UpdateUser(id, updateUser);
//        return result.IsSuccess ? Ok(new
//        {
//            Username = result.Value?.Username,

//        }) : BadRequest(
//            result.Error.description);

//    }
//}