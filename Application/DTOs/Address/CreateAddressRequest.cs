namespace Application.DTOs.Address;

public record CreateAddressRequest(
    string State,
    string City,
    string PostalCode,
    string Street,
    string? AdditionalInfo);
