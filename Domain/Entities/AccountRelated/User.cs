using Domain.Enums.Account;

namespace Domain.Entities.AccountRelated;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateTime? CloseDate { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public UserRole Role { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string Email { get; set; }
    public long? PhoneNumber { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }

    public bool validPassword(string password)
    {
        return true;
    }
}
