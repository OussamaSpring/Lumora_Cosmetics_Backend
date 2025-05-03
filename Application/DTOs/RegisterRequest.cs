using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
<<<<<<< HEAD
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
=======
using System.Text.Json.Serialization;
using Domain.Enums.Account;

namespace Application.DTOs.Category;

//public record RegisterRequest(
//    [property: RegularExpression("^(?=.{6,16}$)(?=(?:.*[A-Za-z]){4})[\\w-]+$\r\n", ErrorMessage = "erorr")]
//        string Username,
//    [property: EmailAddress] string Email,
//    [property: PasswordPropertyText] string Password,
//    [property: Length(4, 50, ErrorMessage = "error")] string FirstName,
//    string? MiddleName,
//    [property: Length(4, 50, ErrorMessage = "error")] string LastName,
//    DateTime? DateOfBirth,
//    Gender? Gender,
//    long? PhoneNumber);


//public record RegisterRequest(
//    [property: Required]
//    [property: RegularExpression("^(?=.{6,16}$)(?=(?:.*[A-Za-z]){4})[\\w-]+$",
//        ErrorMessage = "Username must be 6-16 characters with at least 4 letters")]
//    string Username,

//    [property: Required]
//    [property: EmailAddress(ErrorMessage = "Invalid email format")]
//    string Email,

//    [property: Required]
//    [property: PasswordPropertyText]
//    [property: MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
//    string Password,

//    [property: Required]
//    [property: Length(4, 50, ErrorMessage = "First name must be between 4-50 characters")]
//    string FirstName,

//    [property: JsonPropertyName("middle_name")]
//    string? MiddleName,

//    [property: Required]
//    [property: Length(4, 50, ErrorMessage = "Last name must be between 4-50 characters")]
//    string LastName,

//    DateTime? DateOfBirth,
//    Gender? Gender,

//    [property: Range(1000000000, 9999999999, ErrorMessage = "Invalid phone number")]
//    long? PhoneNumber);

public record RegisterRequest(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string? MiddleName,
    string LastName,
    DateTime? DateOfBirth,
    Gender? Gender,
>>>>>>> origin/main
    long? PhoneNumber);
