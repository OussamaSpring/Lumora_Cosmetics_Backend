using System.ComponentModel.DataAnnotations;
using Domain.Enums.Account;

namespace Application.DTOs;

public class LoginRequest
{
    [Required]
    public string UsernameOrEmail { get; set; }
    [Required]
    public string Password { get; set; }
}


public class LoginResponse
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public AccountStatus AccountStatus { get; set; }
    public UserRole Role { get; set; }
}


public class RegisterRequest
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
    public UserRole Role { get; set; }
}

public class RegisterResponse
{
    public Guid UserID { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
}
