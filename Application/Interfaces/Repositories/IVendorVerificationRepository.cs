using Application.DTOs;
using Domain.Entities.VendorVerification;

public interface IVendorVerificationRepository
{
    Task<VendorProfile?> GetVendorVerificationInfoAsync(Guid vendorId);
    Task<VendorVerificationDocument?> GetVerificationDocumentAsync(int documentId);
    Task<IEnumerable<VendorVerificationDocument>> GetVerificationDocumentsAsync(Guid vendorId);


    Task<int> AddVerificationDocumentAsync(Guid vendorID, VerificationDocumentDto document);


    /* When we approve the document, we need to set the expiration date 
     * (admin responsible for determining the expiration date of the verification document after revising it)
     */
    Task SetVerificationDocumentExpirationDateAsync(int documentId, DateTime expirationDate);
    
    
    // if the vendor is approved, we need to set the expiration date of the document
    Task UpdateVerificationStatusAsync(VerificationStatusUpdateDto update);

    // when the vendor is rejected for any reasong we call on of these methods to delete the documents based on need
    Task DeleteVerificationDocumentAsync(Guid vendorID);
    Task DeleteVerificationDocumentAsync(int documentId);
}