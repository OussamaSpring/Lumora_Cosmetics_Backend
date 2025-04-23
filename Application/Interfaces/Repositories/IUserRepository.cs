using Domain.Entities.AccountRelated;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetProfileAsync(Guid userId);
        //Task<IEnumerable<Address>> GetUserAddressesAsync(Guid userId);
        Task UpdatePersonalInformationAsync(Guid userId, User profile);
        Task UpdateProfileImageAsync(Guid userId, string imageUrl);
        Task UpdateCredentialsAsync(Guid userId, User profile);
        Task DeleteUserAsync(Guid userId);


        //Task<Address?> GetAddressAsync(int addressId);
        //Task<int> AddAddressAsync(Guid userId, Address newAddress);
        //Task DeleteAddressAsync(int addressId);
        //Task UpdateAddressAsync(int addressId, Address address);
    }
}
