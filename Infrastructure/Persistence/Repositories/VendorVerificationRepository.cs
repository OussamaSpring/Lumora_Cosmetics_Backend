using Npgsql;
using Infrastructure.Persistence;


using Domain.Entities.VendorVerification;

using Application.Interfaces;
using Application.DTOs.VendorVerificationDTOs;


public class VendorVerificationRepository : IVendorVerificationRepository
{
    public async Task<VendorProfile> GetVendorVerificationInfoAsync(Guid vendorId)
    {
        using (var connection = DatabaseContext.CreateConnection())
        {
            await connection.OpenAsync();

            const string query = @"
                SELECT * FROM vendor_profile
                WHERE id = @vendorId";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@vendorId", vendorId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new VendorProfile
                        {
                            Id = reader.GetGuid(0),
                            NationalId = reader.GetString(1),
                            TradeId = reader.GetString(2),
                            NationalIdExpirationDate = reader.GetDateTime(3),
                            TradeIdExpirationDate = reader.GetDateTime(4),
                            OtherInfo = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Status = (VerificationStatus)reader.GetInt32(6),
                            VerificationDate = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                            Comments = reader.IsDBNull(8) ? null : reader.GetString(8),
                            UpdateDate = reader.GetDateTime(9),
                            VerifiedByAdmin = reader.IsDBNull(10) ? null : reader.GetGuid(10)
                        };
                    }
                }
            }
        }
        return null;
    }
    
    public async Task<IEnumerable<VendorVerificationDocument>> GetVerificationDocumentsAsync(Guid vendorId)
    {
        var documents = new List<VendorVerificationDocument>();

        using (var connection = DatabaseContext.CreateConnection())
        {
            await connection.OpenAsync();

            const string query = @"
                SELECT 
                    vd.id, vd.document_type, vd.front_document_url, vd.back_document_url, vd.upload_date,
                    vdt.name as document_type_name
                FROM vendor_verification_document vd
                JOIN verification_document_type vdt ON vd.document_type = vdt.id
                WHERE vd.vendor_profile_id = @vendorId
                ORDER BY vd.upload_date DESC";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@vendorId", vendorId);

                using (var reader = await command.ExecuteReaderAsync())
                {
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
                            DocumentTypeName = reader.GetString(5)
                        });
                    }
                }
            }
        }
        return documents;
    }

    public async Task<int> AddVerificationDocumentAsync(VendorVerificationDocument document)
    {
        using (var connection = DatabaseContext.CreateConnection())
        {
            await connection.OpenAsync();

            const string query = @"
                INSERT INTO vendor_verification_document 
                (vendor_profile_id, document_type, front_document_url, back_document_url)
                VALUES (@vendorId, @documentType, @frontUrl, @backUrl)
                RETURNING id";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@vendorId", document.VendorProfileId);
                command.Parameters.AddWithValue("@documentType", document.DocumentTypeId);
                command.Parameters.AddWithValue("@frontUrl", document.FrontDocumentUrl);
                command.Parameters.AddWithValue("@backUrl", (object)document.BackDocumentUrl ?? DBNull.Value);

                return (int)await command.ExecuteScalarAsync();
            }
        }
    }

    public async Task UpdateVerificationStatusAsync(VerificationStatusUpdateDto update)
    {
        using (var connection = DatabaseContext.CreateConnection())
        {
            await connection.OpenAsync();

            const string query = @"
                UPDATE vendor_profile
                SET 
                    verification_status = @status,
                    verification_date = CASE WHEN @status = 2 THEN CURRENT_TIMESTAMP ELSE NULL END,
                    comments = @comments,
                    verified_by_admin = CASE WHEN @status = 2 THEN @adminId ELSE NULL END,
                    update_date = CURRENT_TIMESTAMP
                WHERE id = @vendorId";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@vendorId", update.VendorId);
                command.Parameters.AddWithValue("@status", (int)update.Status);
                command.Parameters.AddWithValue("@comments", (object)update.Comments ?? DBNull.Value);
                command.Parameters.AddWithValue("@adminId", update.AdminId);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<bool> DocumentTypeExistsAsync(int documentTypeId)
    {
        using (var connection = DatabaseContext.CreateConnection())
        {
            await connection.OpenAsync();

            const string query = "SELECT COUNT(1) FROM verification_document_type WHERE id = @typeId";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@typeId", documentTypeId);
                var count = (long)await command.ExecuteScalarAsync();
                return count > 0;
            }
        }
    }

    public async Task<VerificationDocumentType> GetDocumentTypeAsync(short documentTypeId)
    {
        using (var connection = DatabaseContext.CreateConnection())
        {
            await connection.OpenAsync();

            const string query = "SELECT name, specific_notes FROM verification_document_type WHERE id = @typeId";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@typeId", documentTypeId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        return new VerificationDocumentType
                        {
                            Id = documentTypeId,
                            Name = reader.GetString(0),
                            SpecificNotes = reader.IsDBNull(1) ? null : reader.GetString(1)
                        };
                    }
                }
            }
        }
        return null;
    }

}