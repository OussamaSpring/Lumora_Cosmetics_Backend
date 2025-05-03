using Application.DTOs;
<<<<<<< HEAD
=======
using Application.DTOs.Category;
>>>>>>> origin/main
using Domain.Enums.Account;
using Domain.Shared;

namespace Application.Interfaces.Services;

public interface IUserAuthentication
{
<<<<<<< HEAD
    Result<string?> Login(LoginRequest loginRequest);
    Result<string?> Register(RegisterRequest registerRequest, UserRole userRole);
=======
    Task<Result<string?>> Login(LoginRequest loginRequest);
    Task<Result<string?>> Register(RegisterRequest registerRequest, UserRole userRole);
>>>>>>> origin/main
}
