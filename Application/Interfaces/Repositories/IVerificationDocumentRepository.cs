using Domain.Entities.VendorVerification;

namespace Application.Interfaces.Repositories;

public interface IVerificationDocumentRepository
{
    Task<VerificationDocumentType?> GetDocumentTypeAsync(short documentTypeId);
    Task<IEnumerable<VerificationDocumentType>> GetAllDocumentTypesAsync();
    Task<bool> DocumentTypeExistsAsync(int documentTypeId);
}
