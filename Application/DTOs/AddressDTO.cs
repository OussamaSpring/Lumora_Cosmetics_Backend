
namespace Application.DTOs
{
    public class AddressDTO
    {
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string AdditionalInfo { get; set; }
        public bool IsDefault { get; set; }
    }
}
