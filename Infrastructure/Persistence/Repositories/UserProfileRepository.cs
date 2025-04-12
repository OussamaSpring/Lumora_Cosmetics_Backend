
using Npgsql;


using Infrastructure.Persistence;



using Domain.Entities.AccountRelated;
using Domain.Enums.Account;
using Application.DTOs;
using Application.Interfaces.Repositories;

namespace Persistence.Repositories;
public class UserProfileRepository : IUserProfileRepository
{

    private readonly IDbContext _databaseContext;

    public UserProfileRepository(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }


    #region AddData
    
    public async Task<int> AddAddressAsync(Guid userID, Address newAddress)
    {
        using var connection = _databaseContext.CreateConnection();
        const string query1 = @"INSERT INTO address
                           (state, city, postal_code, street, addiontal_info) 
                           VALUES (@state, @city, @postal_code, @street, @addiontal_info)
                           RETURNING id";

        await connection.OpenAsync();
        using var command1 = new NpgsqlCommand(query1, connection);
        command1.Parameters.AddWithValue("@state", newAddress.State);
        command1.Parameters.AddWithValue("@city", newAddress.City);
        command1.Parameters.AddWithValue("@postal_code", newAddress.PostalCode);
        command1.Parameters.AddWithValue("@street", newAddress.Street);
        command1.Parameters.AddWithValue("@addiontal_info", newAddress.AdditionalInfo ?? string.Empty);

        int newAddressId = (int)await command1.ExecuteScalarAsync();


        const string query2 = @"INSERT INTO address_list
                           (user_id, address_id) 
                           VALUES (@userID, @addressID)";

        using var command2 = new NpgsqlCommand(query2, connection);
        command2.Parameters.AddWithValue("@userID", userID);
        command2.Parameters.AddWithValue("@addressID", newAddressId);

        await command2.ExecuteNonQueryAsync();

        return newAddressId;
    }



    #endregion


    #region GetData
    public async Task<User?> GetProfileAsync(Guid userId)
    {
        using (var connection = _databaseContext.CreateConnection())
        {
            await connection.OpenAsync();

            const string query = @"
                SELECT 
                    u.id, u.username, u.password, u.profile_image,
                    u.status, u.role,
                    p.first_name, p.last_name,
                    p.date_of_birth, p.gender, p.email, p.phone_number,
                    p. create_date, p.update_date
                FROM ""user"" u
                JOIN person p ON u.person_id = p.id
                WHERE u.id = @userId";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new User
                        {
                            Id = reader.GetGuid(0),
                            Username = reader.GetString(1),
                            Password = reader.GetString(2),
                            ProfileImageUrl = reader.GetValue(3) != DBNull.Value ? reader.GetString(3) : null,
                            AccountStatus = (AccountStatus)reader.GetInt16(4),
                            Role = (UserRole)reader.GetInt16(5),
                            FirstName = reader.GetString(6),
                            LastName = reader.GetString(7),
                            DateOfBirth = reader.GetValue(8) != DBNull.Value ? reader.GetDateTime(8) : null,
                            Gender = reader.GetString(9).Equals("Male") ? Gender.Male :
                                     (reader.GetString(9).Equals("Female") ? Gender.Female : Gender.Unknown),
                            Email = reader.GetString(10),
                            PhoneNumber = reader.GetValue(11) != DBNull.Value ? reader.GetInt64(11) : null,
                            CreateDate = reader.GetDateTime(12),
                            UpdateDate = reader.GetDateTime(13)
                        };
                    }
                }
            }
        }
        return null;
    }

    public async Task<IEnumerable<Address>> GetUserAddressesAsync(Guid userId)
    {
        var addresses = new List<Address>();
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = "SELECT * FROM address WHERE id = @userId";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@userId", userId);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            addresses.Add(new Address
            {
                Id =  reader.GetInt32(0),
                State = reader.GetString(1),
                City = reader.GetString(2),
                PostalCode = reader.GetString(3),
                Street = reader.GetString(4),
                AdditionalInfo = reader.GetString(5)
            });
        }
        return addresses;
    }

    public async Task<Address?> GetAddressAsync(int addressId)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"SELECT id, state, city, postal_code, street, addiontal_info 
                           FROM address 
                           WHERE id = @addressId";

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



    #endregion



    #region UpdateData
    public async Task UpdateProfileAsync(Guid userId, User profile)
    {
        using var connection = _databaseContext.CreateConnection();
            await connection.OpenAsync();

            const string query = @"
                UPDATE person p
                SET 
                    first_name = @firstName,
                    last_name = @lastName,
                    date_of_birth = @dob,
                    gender = @gender,
                    email = @email,
                    phone_number = @phone,
                    update_date = CURRENT_TIMESTAMP
                FROM ""user"" u
                WHERE u.person_id = p.id AND u.id = @userId";

            using var command = new NpgsqlCommand(query, connection);
            
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@firstName", profile.FirstName);
                command.Parameters.AddWithValue("@lastName", profile.LastName);
                command.Parameters.AddWithValue("@dob", (object)profile.DateOfBirth ?? DBNull.Value);
                command.Parameters.AddWithValue("@gender", (object)profile.Gender ?? DBNull.Value);
                command.Parameters.AddWithValue("@email", profile.Email);
                command.Parameters.AddWithValue("@phone", (object)profile.PhoneNumber ?? DBNull.Value);

                await command.ExecuteNonQueryAsync();

        const string query2 = @"UPDATE ""user"" u
                           SET username = @username,
                                 password = @password,
                               update_date = CURRENT_TIMESTAMP
                           WHERE id = @userId";

        using var command2 = new NpgsqlCommand(query2, connection);
        command2.Parameters.AddWithValue("@userId", userId);
        command2.Parameters.AddWithValue("@username", profile.Username);
        command2.Parameters.AddWithValue("@password", profile.Password);
        await command2.ExecuteNonQueryAsync();

    }
    public async Task UpdateProfileImageAsync(Guid userId, string imageUrl)
    {
        using (var connection = _databaseContext.CreateConnection())
        {
            await connection.OpenAsync();

            const string query = @"
                UPDATE ""user""
                SET profile_image = @imageUrl, update_date = CURRENT_TIMESTAMP
                WHERE id = @userId";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@imageUrl", imageUrl);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateAddressAsync(int addressId, Address address)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"UPDATE address
                           SET state = @state,
                               city = @city,
                               postal_code = @postalCode,
                               street = @street,
                               addiontal_info = @additionalInfo
                           WHERE id = @addressId";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@addressId", addressId);
        command.Parameters.AddWithValue("@state", address.State);
        command.Parameters.AddWithValue("@city", address.City);
        command.Parameters.AddWithValue("@postalCode", address.PostalCode);
        command.Parameters.AddWithValue("@street", address.Street);
        command.Parameters.AddWithValue("@additionalInfo", address.AdditionalInfo ?? string.Empty);

        await command.ExecuteNonQueryAsync();
    }


    #endregion



    #region DeleteData

    public async Task DeleteAddressAsync(int addressId)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query1 = @"DELETE FROM address_list WHERE address_id = @addressId";
        const string query2 = @"DELETE FROM address WHERE id = @addressId";

        using var command1 = new NpgsqlCommand(query1, connection);
        command1.Parameters.AddWithValue("@addressId", addressId);
        await command1.ExecuteNonQueryAsync();

        using var command2 = new NpgsqlCommand(query2, connection);
        command2.Parameters.AddWithValue("@addressId", addressId);
        await command2.ExecuteNonQueryAsync();
    }


    #endregion


    #region CheckData
    #endregion
}