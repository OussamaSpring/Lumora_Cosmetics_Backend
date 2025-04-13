using Npgsql;
using Domain.Entities.AccountRelated;
using Domain.Enums.Account;
using Application.Interfaces.Repositories;


namespace Infrastructure.Persistence.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly IDbContext _databaseContext;
    public AuthRepository(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
    {
        using var connection = _databaseContext.CreateConnection();

        const string query = @"
                SELECT u.id, u.username, u.password, u.profile_image
                u.update_date, u.status, u.role, p.email
                FROM ""user"" u
                JOIN person p ON u.person_id = p.id
                WHERE u.username = @usernameOrEmail OR p.email = @usernameOrEmail";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@usernameOrEmail", usernameOrEmail);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetGuid(0),
                Username = reader.GetString(2),
                Password = reader.GetString(3),
                ProfileImageUrl = reader.GetValue(4) == DBNull.Value ? null : reader.GetString(4),
                UpdateDate = reader.GetDateTime(5),
                AccountStatus = (AccountStatus)reader.GetInt16(6),
                Role = (UserRole)reader.GetInt16(7),
                Email = reader.GetString(8),
            };
        }
        return null;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        using var connection = _databaseContext.CreateConnection();

        const string query = "SELECT COUNT(1) FROM \"user\" WHERE username = @username";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@username", username);

        var count = (long)await command.ExecuteScalarAsync();
        return count > 0;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        using var connection = _databaseContext.CreateConnection();

        const string query = "SELECT COUNT(1) FROM person WHERE email = @email";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@email", email);


        //var IdObject = await command.ExecuteScalarAsync();
        //if (IdObject is null)
        //    return false;

        //Guid id;
        //if (!Guid.TryParse(IdObject.ToString(), out id))
        //    return false;

        //return true;
        var count = (long)await command.ExecuteScalarAsync();
        return count > 0;

    }
    
    public async Task<Guid> CreateUserAsync(User user)
    {
        using var connection = _databaseContext.CreateConnection();

        const string query1 = @"
                INSERT INTO person (first_name, last_name, middle_name, date_of_birth,
                gender, email, phone_number, create_date, update_date)
                VALUES (@firstName, @lastName, @middleName, @dateOfBirth,
                @gender, @email, @phoneNumber, @createDate, @updateDate) 
                RETURNING id";

        await connection.OpenAsync();

        using var command1 = new NpgsqlCommand(query1, connection);
        command1.Parameters.AddWithValue("@firstName", user.FirstName);
        command1.Parameters.AddWithValue("@lastName", user.LastName);
        command1.Parameters.AddWithValue(
            "@middleName", user.MiddleName is null ? DBNull.Value : user.MiddleName);
        command1.Parameters.AddWithValue(
            "@dateOfBirth", user.DateOfBirth is null ? DBNull.Value : user.DateOfBirth);
        command1.Parameters.AddWithValue(
            "@gender", user.Gender is null ? DBNull.Value : user.Gender.ToString());
        command1.Parameters.AddWithValue("@email", user.Email);
        command1.Parameters.AddWithValue(
            "@phoneNumber", user.PhoneNumber is null ? DBNull.Value : user.PhoneNumber);
        command1.Parameters.AddWithValue("@createDate", user.CreateDate);
        command1.Parameters.AddWithValue("@updateDate", user.UpdateDate);

        var personId = (Guid)await command1.ExecuteScalarAsync();
        return personId;

        //const string query2 = @"
        //        INSERT INTO ""user"" (person_id, username, password, profile_image,
        //        update_date, create_date, status, role)
        //        VALUES (@personId, @username, @password, @profileImage,
        //        @updateDate, @createDate, @status, @role) 
        //        RETURNING id";

        //await connection.OpenAsync();
        //using var command = new NpgsqlCommand(query2, connection);
        //command.Parameters.AddWithValue("@personId", personId);
        //command.Parameters.AddWithValue("@username", user.Username);
        //command.Parameters.AddWithValue("@password", user.Password);
        //command.Parameters.AddWithValue(
        //    "@profileImage", user.ProfileImageUrl is null ? DBNull.Value : user.ProfileImageUrl);
        //command.Parameters.AddWithValue("@updateDate", user.UpdateDate);
        //command.Parameters.AddWithValue("@createDate", user.CreateDate);
        //command.Parameters.AddWithValue("@status", (int)user.AccountStatus);
        //command.Parameters.AddWithValue("@role", (int)user.Role);

        //return (Guid)await command.ExecuteScalarAsync();
    }

    public async Task<Admin?> GetAdminByUsernameOrEmailAsync(string usernameOrEmail)
    {
        using var connection = _databaseContext.CreateConnection();    

        const string query = @"
                SELECT a.id, a.person_id, a.username, a.password, a.profile_image, a.update_date, a.status, a.role
                FROM admin a
                JOIN person p ON a.person_id = p.id
                WHERE a.username = @usernameOrEmail OR p.email = @usernameOrEmail";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@usernameOrEmail", usernameOrEmail);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Admin
            {
                Id = reader.GetGuid(0),
                PersonId = reader.GetGuid(1),
                Username = reader.GetString(2),
                Password = reader.GetString(3),
                Profile_Image_URL = reader.GetValue(4) != DBNull.Value ? reader.GetString(4) : null,
                UpdateDate = reader.GetDateTime(4),
                AccountStatus = (AccountStatus)reader.GetInt16(5),

            };
        }

        return null;
    }
}
