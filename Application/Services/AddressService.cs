using Application.DTOs.Address;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities.AccountRelated;
using Domain.Shared;

namespace Application.Services;

public class AddressService : IAddressService
{
    private readonly IUserService _userService;
    private readonly IAddressRepository _addressRepository;
    public AddressService(IUserService userService, IAddressRepository addressRepository)
    {
        _userService = userService;
        _addressRepository = addressRepository;
    }

    public async Task<Result<Address?>> CreateAddress(Guid userId, CreateAddressRequest request)
    {
        var user = _userService.GetUser(userId);
        if (user is null)
        {
            return Result<Address?>
                .Failure(new Error("createAddress", "user does not exist"));
        }

        var address = new Address
        {
            PostalCode = request.PostalCode,
            AdditionalInfo = request.AdditionalInfo,
            City = request.City,
            State = request.State,
            Street = request.Street
        };

        address.Id = await _addressRepository.AddAddressAsync(userId, address);
        return Result<Address?>.Success(address);
    }

    public async Task<Result> DeleteAddress(int id)
    {
        if (await _addressRepository.GetAddressAsync(id) is null)
        {
            return Result
                .Failure(new Error("createAddress", "user does not exist"));
        }
        await _addressRepository.DeleteAddressAsync(id);
        return Result.Success();
    }

    public async Task<Result<Address?>> GetAddressById(int id)
    {
        var address = await _addressRepository.GetAddressAsync(id);
        if (address is null)
            return Result<Address?>.Failure(new Error("ddsd", "dds"));

        return Result<Address?>.Success(address);
    }

    public async Task<Result<Address?>> UpdateAddress(int addressId, UpdateAddressRequest request)
    {
        var address = await _addressRepository.GetAddressAsync(addressId);
        if (address is null)
            return Result<Address?>
                .Failure(new Error("updateAddress", $"does not exist address with {addressId}"));

        address.State = request.State;
        address.Street = request.Street;
        address.City = request.City;
        address.PostalCode = request.PostalCode;
        address.AdditionalInfo = request.AdditionalInfo;

        await _addressRepository.UpdateAddressAsync(addressId, address);
        return Result<Address?>.Success(address);
    }

    public async Task<Result<IEnumerable<Address>>> GetAllAddresses(Guid id)
    {
        return Result<IEnumerable<Address>>
            .Success(await _addressRepository.GetUserAddressesAsync(id));
    }
}
