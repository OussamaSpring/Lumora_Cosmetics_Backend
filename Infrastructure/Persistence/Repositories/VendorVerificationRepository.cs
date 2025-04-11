using Npgsql;
using Infrastructure.Persistence;
using Domain.Entities.VendorVerification;
using Application.DTOs;
using Domain.Enums.enVendorVerification;

public class VendorVerificationRepository : IVendorVerificationRepository
{
    private readonly IDbContext _databaseContext;

    public VendorVerificationRepository(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    #region GetData
    public async Task<VendorProfile?> GetVendorVerificationInfoAsync(Guid vendorId)
    {
        using var connection = _databaseContext.CreateConnection();

        const string query = @"SELECT * FROM vendor_profile
                               WHERE id = @vendorId";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@vendorId", vendorId);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new VendorProfile
            {
                Id = reader.GetGuid(0),
                NationalId = reader.GetString(1),
                TradeId = reader.GetString(2),
                OtherInfo = reader.IsDBNull(3) ? null : reader.GetString(3),
                Status = (VerificationStatus)reader.GetInt16(4),
                VerificationDate = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                Comments = reader.IsDBNull(6) ? null : reader.GetString(6),
                UpdateDate = reader.GetDateTime(7),
                VerifiedByAdmin = reader.IsDBNull(8) ? null : reader.GetGuid(8)
            };
        }
        return null;
    }
    public async Task<VendorVerificationDocument?> GetVerificationDocumentAsync(int documentId)
    {
        using var connection = _databaseContext.CreateConnection();

        const string query = @"SELECT document_type, front_document_url, back_document_url,
                               upload_date, expiration_date
                               FROM vendor_verification_document 
                               WHERE vendor_profile_id = @vendorId
                               ORDER BY upload_date DESC";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@documentId", documentId);
        
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new VendorVerificationDocument
            {
                Id = documentId,
                VendorProfileId = reader.GetGuid(0),
                DocumentTypeId = reader.GetInt16(1),
                FrontDocumentUrl = reader.GetString(2),
                BackDocumentUrl = reader.IsDBNull(3) ? null : reader.GetString(3),
                UploadDate = reader.GetDateTime(4),
                ExpirationDate = reader.GetDateTime(5)
            };
        }
        return null;
    }
    public async Task<IEnumerable<VendorVerificationDocument>> GetVerificationDocumentsAsync(Guid vendorId)
    {
        var documents = new List<VendorVerificationDocument>();
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"SELECT id, document_type, front_document_url, back_document_url,
                               upload_date, expiration_date
                               FROM vendor_verification_document 
                               WHERE vendor_profile_id = @vendorId
                               ORDER BY upload_date DESC";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@vendorId", vendorId);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            documents.Add(new VendorVerificationDocument
            {
                Id = reader.GetInt32(0),
                VendorProfileId = vendorId,
                DocumentTypeId = reader.GetInt16(1),
                FrontDocumentUrl = reader.GetString(2),
                BackDocumentUrl = reader.IsDBNull(3) ? null : reader.GetString(3),
                UploadDate = reader.GetDateTime(4),
                ExpirationDate = reader.GetDateTime(5)
            });
        }
        return documents;
    }
    #endregion

    #region AddData
    public async Task<int> AddVerificationDocumentAsync(Guid vendorID, VerificationDocumentDto document)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"INSERT INTO vendor_verification_document 
                               (vendor_profile_id, document_type, front_document_url, back_document_url)
                               VALUES (@vendorId, @documentType, @frontUrl, @backUrl)
                               RETURNING id";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@vendorId", vendorID);
        command.Parameters.AddWithValue("@documentType", document.DocumentTypeId);
        command.Parameters.AddWithValue("@frontUrl", document.FrontDocumentUrl);
        command.Parameters.AddWithValue("@backUrl", (object)document.BackDocumentUrl ?? DBNull.Value);

        return (int)await command.ExecuteScalarAsync();
    }
    #endregion


    #region UpdateData
    public async Task SetVerificationDocumentExpirationDateAsync(int documentId, DateTime expirationDate)
    {
        using var connection = _databaseContext.CreateConnection();
        const string query = @"UPDATE vendor_verification_document
                               SET expiration_date = @expirationDate
                               WHERE id = @documentId";
    
        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@documentId", documentId);
        command.Parameters.AddWithValue("@expirationDate", expirationDate);

        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateVerificationStatusAsync(VerificationStatusUpdateDto update)
    {
        using var connection = _databaseContext.CreateConnection();

        const string query = @"
                UPDATE vendor_profile
                SET 
                    verification_status = @status,
                    verification_date = CASE WHEN @status = 2 THEN CURRENT_TIMESTAMP ELSE NULL END,
                    comments = @comments,
                    verified_by_admin = CASE WHEN @status = 2 THEN @adminId ELSE NULL END,
                    update_date = CURRENT_TIMESTAMP
                WHERE id = @vendorId";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@vendorId", update.VendorId);
        command.Parameters.AddWithValue("@status", (int)update.Status);
        command.Parameters.AddWithValue("@comments", (object)update.Comments ?? DBNull.Value);
        command.Parameters.AddWithValue("@adminId", update.AdminId);

        await command.ExecuteNonQueryAsync();
    }
    #endregion


    #region CheckData
    public async Task<bool> IsVendorVerifiedAsync(Guid vendorId)
    {
        using var connection = _databaseContext.CreateConnection();
        const string query = "SELECT COUNT(1) FROM vendor_profile WHERE id = @vendorId AND verification_status = 2";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@vendorId", vendorId);
        
        var count = (long)await command.ExecuteScalarAsync();
        return count > 0;
    }
    #endregion


    #region DeleteData
    public async Task DeleteVerificationDocumentAsync(Guid vendorID)
    {
        using var connection = _databaseContext.CreateConnection();
        const string query = @"DELETE FROM vendor_verification_document
                               WHERE vendor_profile_id = @vendorId";
        
        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@vendorId", vendorID);
        
        await command.ExecuteNonQueryAsync();
    }
    public async Task DeleteVerificationDocumentAsync(int documentId)
    {
        using var connection = _databaseContext.CreateConnection();
        const string query = @"DELETE FROM vendor_verification_document
                               WHERE id = @documentId";
        
        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@documentId", documentId);
        
        await command.ExecuteNonQueryAsync();
    }
    #endregion

}