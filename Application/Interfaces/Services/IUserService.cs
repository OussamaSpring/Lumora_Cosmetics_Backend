using Application.DTOs;
using Domain.Shared;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<UserProfileResponse?>> GetUser(Guid id);
    Task<Result<UserProfileResponseWithAddresses?>> GetUserWithAddress(Guid id);
    Task<Result<UserProfileResponse?>> UpdateUser(Guid id, UpdateUserRequest updateUser);
    Task<Result<string?>> UpdateUserPhoto(Guid id);
}