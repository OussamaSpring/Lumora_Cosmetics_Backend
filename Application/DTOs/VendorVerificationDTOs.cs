using Domain.Enums.enVendorVerification;

namespace Application.DTOs;

public class VerificationDocumentDto
{
    public int DocumentTypeId { get; set; }
    public string FrontDocumentUrl { get; set; }
    public string BackDocumentUrl { get; set; }
    public DateTime ExpirationDate { get; set; }
}



public class VendorVerificationInfoDto
{
    public Guid VendorId { get; set; }
    public string NationalId { get; set; }
    public string TradeId { get; set; }
    public VerificationStatus Status { get; set; }
    public DateTime? VerificationDate { get; set; }
    public string Comments { get; set; }
}


public class VerificationStatusUpdateDto
{
    public Guid VendorId { get; set; }
    public VerificationStatus Status { get; set; }
    public string Comments { get; set; }
    public Guid AdminId { get; set; }
}