
using Domain.Entities.AccountRelated;


namespace Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail); // return null if not found
        Task<Admin> GetAdminByUsernameOrEmailAsync(string usernameOrEmail);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<Guid> CreatePersonAsync(Person person);
        Task<Guid> CreateUserAsync(User user);

        // this return username if successfully generated, if not, it throws an Exception
        Task<string> GenerateUniqueUsername(string firstName);
    }
}
