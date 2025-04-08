using Domain.Enums.enVendorVerification;

namespace Domain.Entities.VendorVerification;

public class VendorProfile
{
    public Guid Id { get; set; }
    public string NationalId { get; set; }
    public string TradeId { get; set; }
    public string? OtherInfo { get; set; }
    public VerificationStatus Status { get; set; }
    public DateTime? VerificationDate { get; set; }
    public string? Comments { get; set; }
    public DateTime UpdateDate { get; set; }
    public Guid? VerifiedByAdmin { get; set; }
}
