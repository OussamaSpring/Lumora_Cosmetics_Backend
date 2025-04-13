using Domain.Enums.Account;

namespace Application.DTOs;

public record UpdateUserRequest(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string? MiddleName,
    string LastName,
    DateTime? DateOfBirth,
    long? PhoneNumber,
    Gender? Gender);
