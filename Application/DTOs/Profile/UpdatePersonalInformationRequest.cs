using Domain.Enums.Account;

namespace Application.DTOs.Profile;

public record UpdatePersonalInformationRequest(
    string Username,
    string FirstName,
    string? MiddleName,
    string LastName,
    DateTime? DateOfBirth,
    long? PhoneNumber,
    string? Gender);
