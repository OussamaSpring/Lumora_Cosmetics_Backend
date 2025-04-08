

namespace Domain.Entities.VendorVerification;

public class VendorVerificationDocument
{
    public int Id { get; set; }
    public Guid VendorProfileId { get; set; }
    public short DocumentTypeId { get; set; }
    public string FrontDocumentUrl { get; set; }
    public string? BackDocumentUrl { get; set; }
    public DateTime UploadDate { get; set; }
    public DateTime ExpirationDate { get; set; }
}
