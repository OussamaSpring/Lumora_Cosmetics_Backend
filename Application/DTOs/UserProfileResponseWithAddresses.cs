using Domain.Entities.AccountRelated;
using Domain.Enums.Account;

namespace Application.DTOs;

public class UserProfileResponseWithAddresses
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public long? PhoneNumber { get; set; }
    public Gender? Gender { get; set; }

    public IEnumerable<Address> Addresses { get; set; }
}
