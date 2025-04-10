using Application.DTOs;
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

    public Result<string?> Login(LoginRequest loginRequest)
    {
        try
        {
            var user = _authRepository.GetUserByUsernameOrEmailAsync(loginRequest.UsernameOrEmail).Result;
            if (user is null)
            {
                return Result<string>.Failure(new Error("Login", "this user does'nt exist"));
            }

            if (!(string.Compare(user.Username, loginRequest.UsernameOrEmail, true) == 0 ||
                string.Compare(user.Email, loginRequest.UsernameOrEmail, true) == 0) ||
                !user.validPassword(loginRequest.Password))
            {
                return Result<string>.Failure(new Error("Login", "user name or password is wrong"));
            }

            return Result<string>.Success(_tokenProvider.GenerateToken(user))!;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return Result<string>.Failure(new Error("Internal Server Error", ex.Message));
        }
    }

    public Result<string?> Register(RegisterRequest registerRequest)
    {
        try
        {
            if (!_authRepository.EmailExistsAsync(registerRequest.Email).Result)
            {
                return Result<string>.Failure(new Error("Register", "email already using"));
            }

            var user = new User
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                CreateDate = DateTime.UtcNow,
                Password = registerRequest.Password,
                Role = UserRole.Customer
            };

            var userId = _authRepository.CreateUserAsync(user).Result;
            user.Id = userId;

            return Result<string>.Success(_tokenProvider.GenerateToken(user))!;
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(new Error("Internal Server Error", ex.Message));
        }
    }
}
