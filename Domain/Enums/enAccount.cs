
namespace Domain.Enums.enAccount
{
    public enum AccountStatus
    {
        Active = 1,
        Suspended = 2,
        Closed = 3,
        Pending = 4
    }

    public enum UserRole
    {
        Customer = 1,
        Vendor = 2,
    }

    public enum Gender
    {
        Male = 1,
        Female = 2,
        Unknown = 3
    }

    public enum VerificationStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Under_Review = 4
    }
}