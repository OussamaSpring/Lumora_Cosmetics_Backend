

namespace Application.DTOs.VendorVerificationDTOs
{
    public class VerificationDocumentDto
    {
        public int DocumentTypeId { get; set; }
        public string FrontDocumentUrl { get; set; }
        public string BackDocumentUrl { get; set; }
        public string DocumentTypeName { get; set; }
    }

    public class VerificationStatusUpdateDto
    {
        public Guid VendorId { get; set; }
        public VerificationStatus Status { get; set; }
        public string Comments { get; set; }
        public Guid AdminId { get; set; }
    }

    public class VendorVerificationInfoDto // Vendor Profile response
    {
        public Guid VendorId { get; set; }
        public string NationalId { get; set; }
        public string TradeId { get; set; }
        public DateTime NationalIdExpirationDate { get; set; }
        public DateTime TradeIdExpirationDate { get; set; }
        public VerificationStatus Status { get; set; }
        public DateTime? VerificationDate { get; set; }
        public string Comments { get; set; }
    }
}