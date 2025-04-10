

namespace Domain.Entities.AccountRelated;

    public class Address
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string? AdditionalInfo { get; set; }
    }

