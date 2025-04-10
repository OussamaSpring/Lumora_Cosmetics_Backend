

using Application.DTOs;
using Domain.Entities.AccountRelated;

namespace Application.Interfaces
{
    public interface IUserAddressRepository
    {
        Task<IEnumerable<Address>> GetUserAddressesAsync(Guid userId);
        Task<Address> GetAddressAsync(int addressId);
        Task<int> AddAddressAsync(Guid userId, AddressDTO address);
        Task UpdateAddressAsync(int addressId, AddressDTO address);
        Task DeleteAddressAsync(int addressId);
    }
}
