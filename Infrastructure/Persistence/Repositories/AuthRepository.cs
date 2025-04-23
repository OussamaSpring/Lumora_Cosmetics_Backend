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
        await connection.OpenAsync();

        const string query = @"
                SELECT 
                    u.id, u.username, u.password, u.profile_image,
                    u.status, u.role,
                    p.first_name, p.last_name,
                    p.date_of_birth, p.gender, p.email, p.phone_number,
                    p.create_date, p.update_date
                FROM ""user"" u
                JOIN person p ON u.person_id = p.id
                WHERE u.username = @usernameOrEmail OR
                      p.email = @usernameOrEmail";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@usernameOrEmail", usernameOrEmail);

        using var reader = await command.ExecuteReaderAsync();
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
                Gender = reader.IsDBNull(9) ? null : ParseGender(reader.GetString(8)),
                Email = reader.GetString(10),
                PhoneNumber = reader.GetValue(11) != DBNull.Value ? reader.GetInt64(11) : null,
                CreateDate = reader.GetDateTime(12),
                UpdateDate = reader.GetDateTime(13)
            };
        }
        return null;
    }

    private static Gender? ParseGender(string genderValue)
    {
        if (string.IsNullOrEmpty(genderValue))
            return null;

        return genderValue switch
        {
            "Male" => Gender.Male,
            "Female" => Gender.Female,
            _ => Gender.Unknown
        };
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        using var connection = _databaseContext.CreateConnection();
        const string query = "SELECT COUNT(1) FROM \"user\" WHERE username = @username";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@username", username);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        using var connection = _databaseContext.CreateConnection();
        const string query = "SELECT COUNT(1) FROM person WHERE email = @email";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@email", email);

        return await command.ExecuteNonQueryAsync() > 0;
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
        //command1.Parameters.AddWithValue(
        //    "@gender", user.Gender?.ToString() ?? (object)DBNull.Value);
        command1.Parameters.AddWithValue(
            "@gender", user.Gender is null ? DBNull.Value : user.Gender.Value.ToString());
        command1.Parameters.AddWithValue("@email", user.Email);
        command1.Parameters.AddWithValue(
            "@phoneNumber", user.PhoneNumber is null ? DBNull.Value : user.PhoneNumber);
        command1.Parameters.AddWithValue("@createDate", user.CreateDate);
        command1.Parameters.AddWithValue("@updateDate", user.UpdateDate);

        var personId = (Guid)await command1.ExecuteScalarAsync();
        var id = personId;

        const string query2 = @"
                INSERT INTO ""user"" (id, person_id, username, password, profile_image,
                update_date, close_date, status, role)
                VALUES (@Id, @personId, @username, @password, @profileImage,
                @updateDate, @closeDate, @status, @role)";

        using var command = new NpgsqlCommand(query2, connection);
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@personId", personId);
        command.Parameters.AddWithValue("@username", user.Username);
        command.Parameters.AddWithValue("@password", user.Password);
        command.Parameters.AddWithValue(
            "@profileImage", user.ProfileImageUrl is null ? DBNull.Value : user.ProfileImageUrl);
        command.Parameters.AddWithValue("@updateDate", user.UpdateDate);
        command.Parameters.AddWithValue("@closeDate", DBNull.Value);
        command.Parameters.AddWithValue("@status", (int)user.AccountStatus);
        command.Parameters.AddWithValue("@role", (int)user.Role);
        await command.ExecuteNonQueryAsync();
        return id;
    }


    //public async Task<Guid> CreateUserAsync(User user)
    //{
    //    if (user == null)
    //        throw new ArgumentNullException(nameof(user));

    //    await using var connection = _databaseContext.CreateConnection();
    //    await connection.OpenAsync();

    //    await using var transaction = await connection.BeginTransactionAsync();

    //    try
    //    {
    //        // Insert person record
    //        var personId = await InsertPersonAsync(connection, user);

    //        // Insert user record
    //        var userId = await InsertUserAsync(connection, user, personId);

    //        await transaction.CommitAsync();
    //        return userId;
    //    }
    //    catch (Exception ex)
    //    {
    //        await transaction.RollbackAsync();
    //        throw; // Re-throw with stack trace
    //    }
    //}

    //private async Task<Guid> InsertPersonAsync(NpgsqlConnection connection, User user)
    //{
    //    const string query = @"
    //    INSERT INTO person (
    //        first_name, last_name, middle_name, date_of_birth,
    //        gender, email, phone_number, create_date, update_date
    //    ) VALUES (
    //        @firstName, @lastName, @middleName, @dateOfBirth,
    //        @gender, @email, @phoneNumber, @createDate, @updateDate
    //    ) RETURNING id";

    //    await using var command = new NpgsqlCommand(query, connection);

    //    command.Parameters.AddWithValue("@firstName", user.FirstName ?? throw new ArgumentException("First name is required"));
    //    command.Parameters.AddWithValue("@lastName", user.LastName ?? throw new ArgumentException("Last name is required"));
    //    command.Parameters.AddWithValue("@middleName", (object)user.MiddleName ?? DBNull.Value);
    //    command.Parameters.AddWithValue("@dateOfBirth", (object)user.DateOfBirth ?? DBNull.Value);
    //    command.Parameters.AddWithValue("@gender", (int)user.Gender ?? DBNull.Value);
    //    command.Parameters.AddWithValue("@email", user.Email ?? throw new ArgumentException("Email is required"));
    //    command.Parameters.AddWithValue("@phoneNumber", (object)user.PhoneNumber ?? DBNull.Value);
    //    command.Parameters.AddWithValue("@createDate", user.CreateDate);
    //    command.Parameters.AddWithValue("@updateDate", user.UpdateDate);

    //    return (Guid)await command.ExecuteScalarAsync();
    //}

    //private async Task<Guid> InsertUserAsync(NpgsqlConnection connection, User user, Guid personId)
    //{
    //    const string query = @"
    //    INSERT INTO ""user"" (
    //        person_id, username, password, profile_image,
    //        update_date, close_date, status, role
    //    ) VALUES (
    //        @personId, @username, @password, @profileImage,
    //        @updateDate, @closeDate, @status, @role
    //    ) RETURNING id";

    //    await using var command = new NpgsqlCommand(query, connection);

    //    command.Parameters.AddWithValue("@personId", personId);
    //    command.Parameters.AddWithValue("@username", user.Username ?? throw new ArgumentException("Username is required"));
    //    command.Parameters.AddWithValue("@password", user.Password ?? throw new ArgumentException("Password is required"));
    //    command.Parameters.AddWithValue("@profileImage", (object)user.ProfileImageUrl ?? DBNull.Value);
    //    command.Parameters.AddWithValue("@updateDate", user.UpdateDate);
    //    command.Parameters.AddWithValue("@closeDate", DBNull.Value);
    //    command.Parameters.AddWithValue("@status", (int)user.AccountStatus);
    //    command.Parameters.AddWithValue("@role", (int)user.Role);

    //    return (Guid)await command.ExecuteScalarAsync();
    //}
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
