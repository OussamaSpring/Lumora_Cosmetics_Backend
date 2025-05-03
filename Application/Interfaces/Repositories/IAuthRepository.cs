using Domain.Entities.AccountRelated;

namespace Application.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
    Task<Admin?> GetAdminByUsernameOrEmailAsync(string usernameOrEmail);
    Task<Guid> CreateUserAsync(User user);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
<<<<<<< HEAD
    Task<Guid> CreateUserAsync(User user);

    // this return username if successfully generated, if not, it throws an Exception
    Task<string> GenerateUniqueUsername(string firstName);
=======
>>>>>>> origin/main
}
