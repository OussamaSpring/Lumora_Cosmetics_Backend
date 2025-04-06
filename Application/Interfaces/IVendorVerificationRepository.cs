using Application.DTOs.VendorVerificationDTOs;
using Domain.Entities.VendorVerification;

public interface IVendorVerificationRepository
{
    Task<VendorProfile> GetVendorVerificationInfoAsync(Guid vendorId);
    Task<IEnumerable<VendorVerificationDocument>> GetVerificationDocumentsAsync(Guid vendorId);
    Task<int> AddVerificationDocumentAsync(VendorVerificationDocument document);
    Task UpdateVerificationStatusAsync(VerificationStatusUpdateDto update); // send the dto directly
    Task<bool> DocumentTypeExistsAsync(int documentTypeId);
    Task<VerificationDocumentType> GetDocumentTypeAsync(short documentTypeId);
}