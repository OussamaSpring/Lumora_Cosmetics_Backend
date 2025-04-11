using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Enums.Account;

namespace Application.DTOs;

public record RegisterRequest(
    [property: RegularExpression("^(?=.{6,16}$)(?=(?:.*[A-Za-z]){4})[\\w-]+$\r\n", ErrorMessage = "erorr")]
        string Username,
    [property: EmailAddress] string Email,
    [property: PasswordPropertyText] string Password,
    [property: Length(4, 50, ErrorMessage = "error")] string FirstName, 
    [property: Length(4, 50, ErrorMessage = "error")] string? MiddleName, 
    [property: Length(4, 50, ErrorMessage = "error")] string LastName, 
    DateTime? DateOfBirth, 
    Gender Gender, 
    long? PhoneNumber);
