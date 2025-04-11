using Application.DTOs;
using Domain.Entities.AccountRelated;

namespace Application.Interfaces.Repositories
{
    public interface IUserProfileRepository
    {
        Task<User> GetProfileAsync(Guid userId);
        Task UpdatePersonAsync(Guid userId, ProfileUpdateDto profile);
        Task UpdateProfileImageAsync(Guid userId, string imageUrl);
        Task<bool> EmailExistsAsync(string email, Guid? excludedUserId = null);
        Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid? excludedUserId = null);


        Task<IEnumerable<Address>> GetUserAddressesAsync(Guid userId);
        Task<Address?> GetAddressAsync(int addressId);
        Task<int> AddAddressAsync(Guid userId, AddressDTO newAddress);
        Task UpdateAddressAsync(int addressId, AddressDTO address);
        Task DeleteAddressAsync(int addressId);
    }
}
