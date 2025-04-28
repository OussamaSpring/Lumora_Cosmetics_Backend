using Application.DTOs.Profile;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Enums.Account;
using Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IImageService _imageService;
    private readonly IAddressRepository _addressRepository;
    public UserService(
        IUserRepository userProfileRepository,
        IImageService imageService,
        IAddressRepository addressRepository)
    {
        _userRepository = userProfileRepository;
        _imageService = imageService;
        _addressRepository = addressRepository;
    }

    public async Task<Result<UserProfileResponseWithAddresses?>> GetUserWithAddress(
        Guid id)
    {
        var user = await _userRepository.GetProfileAsync(id);
        if (user is null)
        {
            return Result<UserProfileResponseWithAddresses?>.
                Failure(new Error("User", "user does not exist"));
        }

        var addresses = await _addressRepository.GetUserAddressesAsync(user.Id);

        return Result<UserProfileResponseWithAddresses?>
            .Success(new UserProfileResponseWithAddresses
            {
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl,
                Gender = user.Gender,
                //Addresses = addresses,
            });
    }

    public async Task<Result<UserProfileResponse?>> GetUser(Guid id)
    {
        var user = await _userRepository.GetProfileAsync(id);
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
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl,
                Gender = user.Gender.ToString(),
            });
    }

    public async Task<Result<UserProfileResponse?>> UpdateProfileInformationAsync(
        Guid id, UpdatePersonalInformationRequest request)
    {
        var user = await _userRepository.GetProfileAsync(id);
        if (user is null)
        {
            return Result<UserProfileResponse?>.
                Failure(new Error("User", "user does not exist"));
        }

        user.Username = request.Username;
        user.FirstName = request.FirstName;
        user.MiddleName = request.MiddleName;
        user.LastName = request.LastName;
        user.DateOfBirth = request.DateOfBirth;
        user.PhoneNumber = request.PhoneNumber;
        user.Gender = Convert(request.Gender);
        user.UpdateDate = DateTime.UtcNow;

        await _userRepository.UpdatePersonalInformationAsync(user.Id, user);
        return Result<UserProfileResponse?>.Success(new UserProfileResponse
        {
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            DateOfBirth = user.DateOfBirth,
            PhoneNumber = user.PhoneNumber,
            Gender = user.Gender.ToString(),
            ProfileImageUrl = user.ProfileImageUrl,
        });
    }

    public async Task<Result<string?>> UpdateUserPhoto(Guid id, IFormFile file)
    {
        var user = await _userRepository.GetProfileAsync(id);
        if (user is null)
        {
            return Result<string?>.Failure(new Error("Upload photo", "user does not exist"));
        }

        var uploadImageResult = await _imageService.Upload(file);
        if (uploadImageResult.IsFailure)
        {
            return uploadImageResult;
        }

        user.ProfileImageUrl = uploadImageResult.Value;
        await _userRepository.UpdateProfileImageAsync(id, uploadImageResult.Value);

        return Result<string?>.Success(uploadImageResult.Value);
    }

    public async Task<Result> UpdateCredentialsAsync(
        Guid id,
        UpdateCredentialsRequest request)
    {
        var user = await _userRepository.GetProfileAsync(id);
        if (user is null)
        {
            return Result.
                Failure(new Error("User", "user does not exist"));
        }

        user.Email = request.Email;
        user.Password = HasherSHA256.Hash(request.password);
        user.UpdateDate = DateTime.UtcNow;

        await _userRepository.UpdatePersonalInformationAsync(user.Id, user);
        return Result.Success();

    }

    private Gender Convert(string? value)
    { 
        if (value is null)
            return Gender.Unknown;

        if (value.CompareTo("Male") == 0)
            return Gender.Male;
        if (value.CompareTo("Female") == 0)
            return Gender.Female;
        return Gender.Unknown;
    }

    public async Task<Result> DeleteUserAsync(Guid id)
    {
        //if (await _userRepository.GetProfileAsync(id) is null)
        //{
        //    return Result.Failure(new Error("dad", "dada"));
        //}
        await _userRepository.DeleteUserAsync(id);
        return Result.Success();
    }

    public Task<Result<string?>> UpdateUserPhoto(Guid id)
    {
        throw new NotImplementedException();
    }
}
