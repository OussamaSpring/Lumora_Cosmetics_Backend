using Application.DTOs.Profile;
using Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<UserProfileResponse?>> GetUser(Guid id);
    Task<Result<UserProfileResponseWithAddresses?>> GetUserWithAddress(Guid id);
    Task<Result<UserProfileResponse?>> UpdateProfileInformationAsync(
        Guid id,
        UpdatePersonalInformationRequest request);
    Task<Result> UpdateCredentialsAsync(Guid id, UpdateCredentialsRequest request);
    Task<Result<string?>> UpdateUserPhoto(Guid id, IFormFile file);
    Task<Result> DeleteUserAsync(Guid id);
}