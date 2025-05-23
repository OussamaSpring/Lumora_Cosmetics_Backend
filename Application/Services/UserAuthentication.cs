using Application.DTOs;
using Application.DTOs.Category;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities.AccountRelated;
using Domain.Enums.Account;
using Domain.Shared;

namespace Application.Services;

public class UserAuthentication : IUserAuthentication
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenProvider _tokenProvider;
    public UserAuthentication(IAuthRepository authRepository, ITokenProvider tokenProvider)
    {
        _authRepository = authRepository;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<string?>> Login(LoginRequest loginRequest)
    {
        var user = await _authRepository.GetUserByUsernameOrEmailAsync(loginRequest.UsernameOrEmail);
        if (user is null || !auth(loginRequest, user))
            return Result<string>.Failure(new Error("Login", "user name or password is wrong"));

        return Result<string>.Success(_tokenProvider.GenerateToken(user))!;

        bool auth(LoginRequest loginRequest, User user)
        {
            return ((string.Compare(user.Username, loginRequest.UsernameOrEmail, true) == 0 ||
                     string.Compare(user.Email, loginRequest.UsernameOrEmail, true) == 0) &&
                     user.VirifyPassword(loginRequest.Password));
        }
    }

    public async Task<Result<string?>> Register(RegisterRequest register, UserRole userRole)
    {
        try
        {
            if (await _authRepository.UsernameExistsAsync(register.Username))
                return Result<string>.Failure(new Error("Register", "username already using"));

            // exception in unique constraint ???
            if (await _authRepository.EmailExistsAsync(register.Email))
                return Result<string>.Failure(new Error("Register", "email already using"));

            var user = new User
            {
                Username = register.Username,
                Email = register.Email,
                Password = HasherSHA256.Hash(register.Password),
                FirstName = register.FirstName,
                MiddleName = register.MiddleName,
                LastName = register.LastName,
                DateOfBirth = register.DateOfBirth,
                PhoneNumber = register.PhoneNumber,
                Gender =  Convert(register.Gender),
                ProfileImageUrl = null,
                Role = userRole,
                AccountStatus = AccountStatus.Active,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };

            var userId = await _authRepository.CreateUserAsync(user);
            user.Id = userId;

            return Result<string>.Success(_tokenProvider.GenerateToken(user))!;
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(new Error("Internal Server Error", ex.Message));
        }
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
}
