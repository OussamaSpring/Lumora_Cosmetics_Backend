using Application.DTOs;
using Domain.Enums.Account;
using Domain.Shared;

namespace Application.Interfaces.Services;

public interface IUserAuthentication
{
    Task<Result<string?>> Login(LoginRequest loginRequest);
    Task<Result<string?>> Register(RegisterRequest registerRequest, UserRole userRole);
}
