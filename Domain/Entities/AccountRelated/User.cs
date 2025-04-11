using Domain.Enums.Account;

namespace Domain.Entities.AccountRelated;

public class User : Person
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateTime? CloseDate { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public UserRole Role { get; set; }

    public bool validPassword(string password)
    {
        return true;
    }
}
