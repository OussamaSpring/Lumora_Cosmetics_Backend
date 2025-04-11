using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Enums.Account;

namespace Application.DTOs;

public class LoginRequest
{
    // should add regex
    public required string UsernameOrEmail { get; set; }
    
    [PasswordPropertyText]
    public required string Password { get; set; }
}


public class RegisterRequest
{
    [RegularExpression("^(?=.{6,16}$)(?=(?:.*[A-Za-z]){4})[\\w-]+$\r\n", ErrorMessage = "erorr")]
    public required string Username { get; set; }

    [EmailAddress]
    public required string Email { get; set; }
    
    [PasswordPropertyText]
    public required string Password { get; set; }

    [Length(4, 50, ErrorMessage = "error")] // should rewrite errors
    public required string FirstName { get; set; }
    
    [Length(4, 50, ErrorMessage = "error")]
    public string? MiddleName { get; set; }
    
    [Length(4, 50, ErrorMessage = "error")]
    public required string LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public required Gender Gender { get; set; }
    
    public long? PhoneNumber { get; set; }
}
