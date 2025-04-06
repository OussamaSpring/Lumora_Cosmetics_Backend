using Domain.Enums.enAccount;


namespace Domain.Entities.AccountRelated
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public long? PhoneNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
    public class Address
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string? AdditionalInfo { get; set; }
    }

    // this is for multiple addresses
    public class AddressList
    {
        public Guid PersonId { get; set; }
        public int AddressId { get; set; }
    }
    public class Admin : Person
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Profile_Image_URL { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }

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
}