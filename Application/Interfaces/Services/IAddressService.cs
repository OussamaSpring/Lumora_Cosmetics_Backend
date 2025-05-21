using Application.DTOs.Address;
using Domain.Entities.AccountRelated;
using Domain.Shared;

namespace Application.Interfaces.Services;

public interface IAddressService
{
    Task<Result<Address?>> GetAddressById(int id);
    Task<Result<Address>> CreateAddress(Guid userId, CreateAddressRequest address);
    Task<Result> DeleteAddress(int id);
    Task<Result<Address>> UpdateAddress(int addressId, UpdateAddressRequest address);
    Task<Result<IEnumerable<Address>>> GetAllAddresses(Guid id);
}
