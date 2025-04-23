namespace Application.DTOs.Address;

public record UpdateAddressRequest(
    string State,
    string City,
    string PostalCode,
    string Street,
    string? AdditionalInfo);
