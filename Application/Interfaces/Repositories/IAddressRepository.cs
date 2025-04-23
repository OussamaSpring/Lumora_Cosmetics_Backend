using Domain.Entities.AccountRelated;

namespace Application.Interfaces.Repositories;

public interface IAddressRepository
{
    Task<IEnumerable<Address>> GetUserAddressesAsync(Guid userId);
    Task<Address?> GetAddressAsync(int addressId);
    Task<int> AddAddressAsync(Guid userId, Address newAddress);
    Task DeleteAddressAsync(int addressId);
    Task UpdateAddressAsync(int addressId, Address address);
}