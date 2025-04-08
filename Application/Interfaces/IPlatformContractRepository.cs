
using Domain.Entities.VendorVerification;

namespace Application.Interfaces
{
    public interface IPlatformContractRepository
    {
        Task<Contract> GetContractAsync(short contractId);
        Task<IEnumerable<Contract>> GetAllContractsAsync();

        Task<IEnumerable<VendorContract>> GetVendorContractsAsync(Guid vendorId);
        Task <VendorContract> GetVendorContractAsync(Guid vendorId, short contractId);

        Task<IEnumerable<VendorContract>> GetVendorContractsAsync(short contractId);

        Task AddVendorContractAsync(Guid vendorId, short contractId, string signature);


        Task DeleteVendorContractAsync(Guid vendorId, short contractId);
    }
}
