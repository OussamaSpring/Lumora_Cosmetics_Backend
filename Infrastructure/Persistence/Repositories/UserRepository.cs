using Npgsql;
using Infrastructure.Persistence;
using Domain.Entities.AccountRelated;
using Domain.Enums.Account;
using Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Repositories;
public class UserRepository : IUserRepository
{
    private readonly IDbContext _databaseContext;
    public UserRepository(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<User?> GetProfileAsync(Guid userId)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"
                SELECT 
                    u.id, u.username, u.profile_image,
                    u.status, u.role,
                    p.first_name, p.last_name,
                    p.date_of_birth, p.gender, p.email, p.phone_number,
                    p.create_date, p.update_date
                FROM ""user"" u
                JOIN person p ON u.person_id = p.id
                WHERE u.id = @userId";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@userId", userId);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            //Console.WriteLine("dadadadadDAD" + Convert(reader.GetString(8)));
            return new User
            {
                Id = reader.GetGuid(0),
                Username = reader.GetString(1),
                ProfileImageUrl = reader.GetValue(2) != DBNull.Value ? reader.GetString(2) : null,
                AccountStatus = (AccountStatus)reader.GetInt16(3),
                Role = (UserRole)reader.GetInt16(4),
                FirstName = reader.GetString(5),
                LastName = reader.GetString(6),
                DateOfBirth = reader.GetValue(7) != DBNull.Value ? reader.GetDateTime(7) : null,
                Gender = reader.GetValue(8) != DBNull.Value ? Convert(reader.GetString(8)) : Convert(null),
                Email = reader.GetString(9),
                PhoneNumber = reader.GetValue(10) != DBNull.Value ? reader.GetInt64(11) : null,
                CreateDate = reader.GetDateTime(11),
                UpdateDate = reader.GetDateTime(12)
            };
        }
        return null;
    }

    public async Task UpdatePersonalInformationAsync(Guid userId, User user)
    {
        using var connection = _databaseContext.CreateConnection();
        const string sql = @"UPDATE person
                             SET first_name = @FirstName, last_name = @LastName,
                             middle_name = @MiddleName, date_of_birth = @DateOfBirth,
                             gender = @Gender, phone_number = @PhoneNumber,
                             update_date = @UpdateDate 
                            WHERE id = @Id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@FirstName", user.FirstName);
        command.Parameters.AddWithValue("@LastName", user.LastName);
        command.Parameters.AddWithValue("@MiddleName", 
            user.MiddleName is null ? DBNull.Value : user.MiddleName);
        command.Parameters.AddWithValue("@DateOfBirth",
            user.DateOfBirth is null ? DBNull.Value : user.DateOfBirth);
        command.Parameters.AddWithValue("@Gender",
            user.Gender is not null ? user.Gender.Value.ToString() : DBNull.Value);
        command.Parameters.AddWithValue("@PhoneNumber",
            user.PhoneNumber is null ? DBNull.Value : user.PhoneNumber);
        command.Parameters.AddWithValue("@UpdateDate", user.UpdateDate);
        command.Parameters.AddWithValue("@Id", userId);
        command.ExecuteNonQuery();
    }

    public Task UpdateProfileImageAsync(Guid userId, string imageUrl)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateCredentialsAsync(Guid userId, User user)
    {
        using var connection = _databaseContext.CreateConnection();
        const string sql1 = @"UPDATE ""user""
                             SET password = @Password
                            WHERE id = @Id";

        const string sql2 = @"UPDATE person
                             SET email = @Email
                            WHERE id = @Id";

        await connection.OpenAsync();

        using var command1 = new NpgsqlCommand(sql1, connection);
        command1.Parameters.AddWithValue("@Password", user.Password);
        using var command2 = new NpgsqlCommand(sql2, connection);
        command2.Parameters.AddWithValue("@Email", user.Email);


        await command1.ExecuteNonQueryAsync();
        await command2.ExecuteNonQueryAsync();
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        using var connection = _databaseContext.CreateConnection();
        const string sql= @"DELETE FROM ""user"" WHERE id = @Id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", userId);
        await command.ExecuteNonQueryAsync();
    }

    private Gender Convert(string? value)
    {
        if (value is null)
            return Gender.Unknown;

        if (value.CompareTo("Male") == 0)
            return Gender.Male;
        if (value.CompareTo("Female") == 0)
            return Gender.Female;
        return Gender.Unknown;
    }

}