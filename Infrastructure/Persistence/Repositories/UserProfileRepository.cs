
using Npgsql;


using Infrastructure.Persistence;



using Domain.Entities.AccountRelated;
using Domain.Enums.enAccount;
using Application.Interfaces;
using Application.DTOs;

namespace Persistence.Repositories;
public class UserProfileRepository : IUserProfileRepository
{
    #region GetData
    public async Task<User> GetProfileAsync(Guid userId)
    {
        using (var connection = DatabaseContext.CreateConnection())
        {
            await connection.OpenAsync();

            const string query = @"
                SELECT 
                    u.id, p.id, p.first_name, p.last_name, p.middle_name, 
                    p.date_of_birth, p.gender, p.email, p.phone_number,
                    u.profile_image
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
                            PersonId = reader.GetGuid(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            MiddleName = reader.IsDBNull(4) ? null : reader.GetString(4),
                            DateOfBirth = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                            Gender = reader.IsDBNull(6) ? null : (Gender)reader.GetInt16(6),
                            Email = reader.GetString(7),
                            PhoneNumber = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                            ProfileImageURL = reader.IsDBNull(9) ? null : reader.GetString(9)
                        };
                    }
                }
            }
        }
        return null;
    }

    public async Task UpdateProfileAsync(Guid userId, ProfileUpdateDto profile)
    {
        using (var connection = DatabaseContext.CreateConnection())
        {
            await connection.OpenAsync();

            const string query = @"
                UPDATE person p
                SET 
                    first_name = @firstName,
                    last_name = @lastName,
                    middle_name = @middleName,
                    date_of_birth = @dob,
                    gender = @gender,
                    email = @email,
                    phone_number = @phone,
                    update_date = CURRENT_TIMESTAMP
                FROM ""user"" u
                WHERE u.person_id = p.id AND u.id = @userId";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@firstName", profile.FirstName);
                command.Parameters.AddWithValue("@lastName", profile.LastName);
                command.Parameters.AddWithValue("@middleName", (object)profile.MiddleName ?? DBNull.Value);
                command.Parameters.AddWithValue("@dob", (object)profile.DateOfBirth ?? DBNull.Value);
                command.Parameters.AddWithValue("@gender", (object)profile.Gender ?? DBNull.Value);
                command.Parameters.AddWithValue("@email", profile.Email);
                command.Parameters.AddWithValue("@phone", (object)profile.PhoneNumber ?? DBNull.Value);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    #endregion

    #region UpdateData
    public async Task UpdateProfileImageAsync(Guid userId, string imageUrl)
    {
        using (var connection = DatabaseContext.CreateConnection())
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

    #endregion

    #region CheckData
    public async Task<bool> EmailExistsAsync(string email, Guid? excludedUserId = null)
    {
        using (var connection = DatabaseContext.CreateConnection())
        {
            await connection.OpenAsync();

            var query = @"
                SELECT COUNT(1) 
                FROM person p
                JOIN ""user"" u ON p.id = u.person_id
                WHERE p.email = @email";

            if (excludedUserId.HasValue)
            {
                query += " AND u.id != @excludedUserId";
            }

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email", email);
                if (excludedUserId.HasValue)
                {
                    command.Parameters.AddWithValue("@excludedUserId", excludedUserId.Value);
                }

                var count = (long)await command.ExecuteScalarAsync();
                return count > 0;
            }
        }
    }


    public async Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid? excludedUserId = null)
    {
        using (var connection = DatabaseContext.CreateConnection())
        {
            await connection.OpenAsync();

            var query = @"
                SELECT COUNT(1) 
                FROM person p
                JOIN ""user"" u ON p.id = u.person_id
                WHERE p.phone_number = @phone";

            if (excludedUserId.HasValue)
            {
                query += " AND u.id != @excludedUserId";
            }

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@phone", phoneNumber);
                if (excludedUserId.HasValue)
                {
                    command.Parameters.AddWithValue("@excludedUserId", excludedUserId.Value);
                }

                var count = (long)await command.ExecuteScalarAsync();
                return count > 0;
            }
        }
    }
    #endregion
}