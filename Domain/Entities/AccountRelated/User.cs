using Domain.Enums.Account;
using Domain.Shared;

namespace Domain.Entities.AccountRelated;

public class User
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateTime UpdateDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public UserRole Role { get; set; }
    public Guid PersonId { get; set; }

    public bool VirifyPassword(string password)
    {
        return string.Compare(Password, HasherSHA256.Hash(password), false) == 0;
    }
}
