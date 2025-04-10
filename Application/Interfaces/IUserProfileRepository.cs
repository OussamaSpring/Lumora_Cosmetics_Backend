

using Application.DTOs;
using Domain.Entities.AccountRelated;

namespace Application.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<User> GetProfileAsync(Guid userId);
        Task UpdateProfileAsync(Guid userId, ProfileUpdateDto profile);
        Task UpdateProfileImageAsync(Guid userId, string imageUrl);
        Task<bool> EmailExistsAsync(string email, Guid? excludedUserId = null);
        Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid? excludedUserId = null);

    }
}
