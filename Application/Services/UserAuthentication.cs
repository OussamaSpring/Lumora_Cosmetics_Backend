using System.Security.Cryptography.X509Certificates;
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
                return Result<string>.Failure(new Error("Login", "this user does not exist"));
            }


            if (!auth(loginRequest, user))
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

        static bool auth(LoginRequest loginRequest, User user)
        {
            return ((string.Compare(user.Username, loginRequest.UsernameOrEmail, true) == 0 ||
                     string.Compare(user.Email, loginRequest.UsernameOrEmail, true) == 0) &&
                     user.validPassword(loginRequest.Password));
        }
    }

    public Result<string?> Register(RegisterRequest registerRequest, UserRole userRole)
    {
        try
        {
            if (_authRepository.EmailExistsAsync(registerRequest.Email).Result)
            {
                return Result<string>.Failure(new Error("Register", "email already using"));
            }
            if (_authRepository.UsernameExistsAsync(registerRequest.Username).Result)
            {
                return Result<string>.Failure(new Error("Register", "username already using"));
            }

            var person = new Person
            {
                FirstName = registerRequest.FirstName,
                MiddleName = registerRequest.MiddleName,
                LastName = registerRequest.LastName,
                DateOfBirth = registerRequest.DateOfBirth,
                PhoneNumber = registerRequest.PhoneNumber,
                Gender = registerRequest.Gender,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };

            var personId = _authRepository.CreatePersonAsync(person).Result;
            person.PersonId = personId;

            var user = new User
            {
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                ProfileImageUrl = string.Empty, // profile image (nullable)
                Role = userRole,
                AccountStatus = AccountStatus.Active,
                UpdateDate = DateTime.Now,
                PersonId = personId,
            };

            var userId = _authRepository.CreateUserAsync(user).Result;
            user.UserId = userId;

            return Result<string>.Success(_tokenProvider.GenerateToken(user))!;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return Result<string>.Failure(new Error("Internal Server Error", ex.Message));
        }
    }
}
