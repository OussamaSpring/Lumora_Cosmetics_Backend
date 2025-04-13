using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Shared;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IImageService _imageService;
    public UserService(IUserProfileRepository userProfileRepository, IImageService imageService)
    {
        _userProfileRepository = userProfileRepository;
        _imageService = imageService;
    }

    public async Task<Result<UserProfileResponseWithAddresses?>> GetUserWithAddress(
        Guid id)
    {
        var user = await _userProfileRepository.GetProfileAsync(id);
        if (user is null)
        {
            return Result<UserProfileResponseWithAddresses?>.
                Failure(new Error("User", "user does not exist"));
        }

        var addresses = await _userProfileRepository.GetUserAddressesAsync(user.Id);

        return Result<UserProfileResponseWithAddresses?>
            .Success(new UserProfileResponseWithAddresses
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl,
                Gender = user.Gender,
                Addresses = addresses,
            });
    }

    public async Task<Result<UserProfileResponse?>> GetUser(
    Guid id)
    {
        var user = await _userProfileRepository.GetProfileAsync(id);
        if (user is null)
        {
            return Result<UserProfileResponse?>.
                Failure(new Error("User", "user does not exist"));
        }

        return Result<UserProfileResponse?>
            .Success(new UserProfileResponse
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl,
                Gender = user.Gender,
            });
    }

    public async Task<Result<UserProfileResponse?>> UpdateUser(
        Guid id, UpdateUserRequest updateUser)
    {
        var user = await _userProfileRepository.GetProfileAsync(id);
        if (user is null)
        {
            return Result<UserProfileResponse?>.
                Failure(new Error("User", "user does not exist"));
        }

        user.Username = updateUser.Username;
        user.Email = updateUser.Email;
        user.Password = updateUser.Password;
        user.FirstName = updateUser.FirstName;
        user.MiddleName = updateUser.MiddleName;
        user.LastName = updateUser.LastName;
        user.DateOfBirth = updateUser.DateOfBirth;
        user.PhoneNumber = updateUser.PhoneNumber;
        user.Gender = updateUser.Gender;
        user.UpdateDate = DateTime.UtcNow;

        await _userProfileRepository.UpdateProfileAsync(user.Id, user);
        return Result<UserProfileResponse?>.Success(new UserProfileResponse
        {
            Username = updateUser.Username,
            Email = updateUser.Email,
            Password = updateUser.Password,
            FirstName = updateUser.FirstName,
            MiddleName = updateUser.MiddleName,
            LastName = updateUser.LastName,
            DateOfBirth = updateUser.DateOfBirth,
            PhoneNumber = updateUser.PhoneNumber,
            Gender = updateUser.Gender,
        });
    }

    public async Task<Result<string?>> UpdateUserPhoto(Guid id) // photo
    {
        var user = await _userProfileRepository.GetProfileAsync(id);
        if (user is null)
        {
            return Result<string?>.Failure(new Error("Upload photo", "user does not exist"));
        }

        var url = _imageService.Upload(string.Empty);
        if (url.Value == null)
        {
            return Result<string>.Failure(new Error("Upload photo", "upload photo"));
        }

        user.ProfileImageUrl = url.Value;
        await _userProfileRepository.UpdateProfileImageAsync(id, url.Value);

        return Result<string?>.Success(url.Value);
    }
}
