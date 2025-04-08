using Domain.Enums.enAccount;


namespace Domain.Entities.AccountRelated;

public class User : Person
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string? Profile_Image_URL { get; set; }
    public DateTime UpdateDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public UserRole Role { get; set; }
}
