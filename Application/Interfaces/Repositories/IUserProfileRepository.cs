using Application.DTOs;
using Domain.Entities.AccountRelated;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserProfileRepository
    {
        Task<User?> GetProfileAsync(Guid userId); // Get all user data
        Task<IEnumerable<Address>> GetUserAddressesAsync(Guid userId);
        
        Task UpdateProfileAsync(Guid userId, User profile); // update all user data except password and image
        Task UpdateProfileImageAsync(Guid userId, string imageUrl);
        Task UpdateAddressAsync(int addressId, Address address);

        Task<Address?> GetAddressAsync(int addressId);
        Task<int> AddAddressAsync(Guid userId, Address newAddress);
        Task DeleteAddressAsync(int addressId);
    }
}
