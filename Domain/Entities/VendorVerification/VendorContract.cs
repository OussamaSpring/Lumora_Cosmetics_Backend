

namespace Domain.Entities.VendorVerification;

public class VendorContract
{
    public Guid VendorId { get; set; }
    public short ContractId { get; set; }
    public string Signature { get; set; }
    public DateTime SignedDate { get; set; }
}