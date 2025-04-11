using Application.DTOs;
using Domain.Enums.Account;
using Domain.Shared;

namespace Application.Interfaces.Services;

public interface IUserAuthentication
{
    Result<string?> Login(LoginRequest loginRequest);
    Result<string?> Register(RegisterRequest registerRequest, UserRole userRole);
}
