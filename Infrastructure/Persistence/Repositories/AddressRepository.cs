using Application.Interfaces.Repositories;
using Domain.Entities.AccountRelated;
using Npgsql;

namespace Infrastructure.Persistence.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly IDbContext _databaseContext;
    public AddressRepository(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IEnumerable<Address>> GetUserAddressesAsync(Guid userId)
    {
        using var connection = _databaseContext.CreateConnection();
        const string query = @"SELECT id, state, city, postal_code, street, additional_info
                                FROM address join address_list
                            ON id = address_id WHERE person_id = @personId";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@personId", userId);   

        using var reader = await command.ExecuteReaderAsync();

        var addresses = new List<Address>();
        while (await reader.ReadAsync())
        {
            addresses.Add(new Address
            {
                Id = reader.GetInt32(0),
                State = reader.GetString(1),
                City = reader.GetString(2),
                PostalCode = reader.GetString(3),
                Street = reader.GetString(4),
                AdditionalInfo = reader.IsDBNull(5) ? null : reader.GetString(5),
            });
        }
        return addresses;
    }

    public async Task<Address?> GetAddressAsync(int addressId)
    {
        using var connection = _databaseContext.CreateConnection();
        const string query = @"SELECT id, state, city, postal_code, street, additional_info 
                           FROM address 
                           WHERE id = @addressId";
        
        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@addressId", addressId);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Address
            {
                Id = reader.GetInt32(0),
                State = reader.GetString(1),
                City = reader.GetString(2),
                PostalCode = reader.GetString(3),
                Street = reader.GetString(4),
                AdditionalInfo = reader.IsDBNull(5) ? null : reader.GetString(5)
            };
        }

        return null;
    }

    public async Task<int> AddAddressAsync(Guid userId, Address newAddress)
    {
        using var connection = _databaseContext.CreateConnection();
        const string query1 = @"INSERT INTO address
                           (state, city, postal_code, street, additional_info) 
                           VALUES (@state, @city, @postal_code, @street, @additional_info)
                           RETURNING id";

        await connection.OpenAsync();
        using var command1 = new NpgsqlCommand(query1, connection);
        command1.Parameters.AddWithValue("@state", newAddress.State);
        command1.Parameters.AddWithValue("@city", newAddress.City);
        command1.Parameters.AddWithValue("@postal_code", newAddress.PostalCode);
        command1.Parameters.AddWithValue("@street", newAddress.Street);
        command1.Parameters.AddWithValue("@additional_info", newAddress.AdditionalInfo ?? string.Empty);

        int addressId = (int)await command1.ExecuteScalarAsync();

        const string query2 = @"INSERT INTO address_list
                           (person_id, address_id) 
                           VALUES (@personId, @addressId)";

        using var command2 = new NpgsqlCommand(query2, connection);
        command2.Parameters.AddWithValue("@personId", userId);
        command2.Parameters.AddWithValue("@addressId", addressId);

        await command2.ExecuteNonQueryAsync();
        return addressId;
    }

    public async Task DeleteAddressAsync(int addressId)
    {
        using var connection = _databaseContext.CreateConnection();

        const string query1 = @"DELETE FROM address_list WHERE address_id = @addressId";
        const string query2 = @"DELETE FROM address WHERE id = @addressId";

        await connection.OpenAsync();
        using var command1 = new NpgsqlCommand(query1, connection);
        command1.Parameters.AddWithValue("@addressId", addressId);
        await command1.ExecuteNonQueryAsync();

        using var command2 = new NpgsqlCommand(query2, connection);
        command2.Parameters.AddWithValue("@addressId", addressId);
        await command2.ExecuteNonQueryAsync();
    }

    public async Task UpdateAddressAsync(int addressId, Address address)
    {
        using var connection = _databaseContext.CreateConnection();
        const string sql = @"UPDATE address
                            SET state = @State, city = @City,
                            postal_code = @PostalCode, street = @Street,
                            additional_info = @AdditionalInfo
                            WHERE id = @AddressId";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@State", address.State);
        command.Parameters.AddWithValue("@City", address.City);
        command.Parameters.AddWithValue("@PostalCode", address.PostalCode);
        command.Parameters.AddWithValue("@Street", address.Street);
        command.Parameters.AddWithValue("@AdditionalInfo",
            address.AdditionalInfo is null ? DBNull.Value : address.AdditionalInfo);
        command.Parameters.AddWithValue("@AddressId", addressId);

        await command.ExecuteNonQueryAsync();
    }
}
