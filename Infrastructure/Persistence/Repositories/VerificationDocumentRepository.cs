﻿using Npgsql;

using Domain.Entities.VendorVerification;
using Application.Interfaces.Repositories;

namespace Infrastructure.Persistence.Repositories;

class VerificationDocumentRepository : IVerificationDocumentRepository
{
    private readonly IDbContext _databaseContext;

    public VerificationDocumentRepository(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<VerificationDocumentType?> GetDocumentTypeAsync(short documentTypeId)
    {
        using var connection = _databaseContext.CreateConnection();

        const string query = @"SELECT name, specific_notes FROM verification_document_type
                               WHERE id = @typeId";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@typeId", documentTypeId);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new VerificationDocumentType
            {
                Id = documentTypeId,
                Name = reader.GetString(0),
                SpecificNotes = reader.IsDBNull(1) ? null : reader.GetString(1)
            };
        }
        return null;
    }
    
    public async Task<IEnumerable<VerificationDocumentType>> GetAllDocumentTypesAsync()
    {
        var documentTypes = new List<VerificationDocumentType>();
        using var connection = _databaseContext.CreateConnection();

        const string query = "SELECT * FROM verification_document_type";
        
        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            documentTypes.Add(new VerificationDocumentType
            {
                Id = reader.GetInt16(0),
                Name = reader.GetString(1),
                SpecificNotes = reader.IsDBNull(2) ? null : reader.GetString(2)
            });
        }
        return documentTypes;
    }

    public async Task<bool> DocumentTypeExistsAsync(int documentTypeId)
    {
        using var connection = _databaseContext.CreateConnection();

        const string query = @"SELECT COUNT(1) FROM verification_document_type
                               WHERE id = @typeId";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@typeId", documentTypeId);
        
        var count = (long)await command.ExecuteScalarAsync();
        return count > 0;
    }
}