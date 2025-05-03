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
                SELECT u.id, u.person_id, u.username, u.password, u.profile_image, u.update_date, u.status, u.role
                FROM ""user"" u
                JOIN person p ON u.person_id = p.id
                WHERE u.username = @usernameOrEmail OR
                      p.email = @usernameOrEmail";

                await connection.OpenAsync();
                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@usernameOrEmail", usernameOrEmail);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User { 
                            Id = reader.GetGuid(0),
                            PersonId = reader.GetGuid(1),
                            Username = reader.GetString(2),
                            Password = reader.GetString(3),
                            ProfileImageUrl = reader.GetValue(4) != DBNull.Value ? reader.GetString(4) : null,
                            UpdateDate = reader.GetDateTime(5),
                            AccountStatus = (AccountStatus)reader.GetInt16(6),
                            Role = (UserRole)reader.GetInt16(7)
                            };
                        }
                    }
                
            

        return null;
    }

        public async Task<Admin> GetAdminByUsernameOrEmailAsync(string usernameOrEmail)
        {
        using var connection = _databaseContext.CreateConnection();
            
                await connection.OpenAsync();

                const string query = @"
                SELECT a.id, a.person_id, a.username, a.password, a.profile_image, a.update_date, a.status, a.role
                FROM admin a
                JOIN person p ON a.person_id = p.id
                WHERE a.username = @usernameOrEmail OR p.email = @usernameOrEmail";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@usernameOrEmail", usernameOrEmail);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
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

        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        using var connection = _databaseContext.CreateConnection();
        const string query = "SELECT COUNT(1) FROM person WHERE email = @email";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@email", email);

        var count = (long)await command.ExecuteScalarAsync();
        return count > 0;
    }

    public async Task<Guid> CreatePersonAsync(Person person)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query = @"
                INSERT INTO person (first_name, last_name, email)
                VALUES (@firstName, @lastName, @email)
                RETURNING id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@firstName", person.FirstName);
        command.Parameters.AddWithValue("@lastName", person.LastName);
        command.Parameters.AddWithValue("@email", person.Email);

        return (Guid)await command.ExecuteScalarAsync();
    }

    public async Task<Guid> CreateUserAsync(User user)
    {
        using var connection = _databaseContext.CreateConnection();

        const string query = @"
                INSERT INTO ""user"" (person_id, username, password, status, role)
                VALUES (@personId, @username, @password, @status, @role) 
                RETURNING id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@personId", user.PersonId);
        command.Parameters.AddWithValue("@username", user.Username);
        command.Parameters.AddWithValue("@password", user.Password);
        command.Parameters.AddWithValue(
            "@profileImage", user.ProfileImageUrl is null ? DBNull.Value : user.ProfileImageUrl);
        command.Parameters.AddWithValue("@updateDate", user.UpdateDate);
        command.Parameters.AddWithValue("@closeDate", DBNull.Value);
        command.Parameters.AddWithValue("@status", (int)user.AccountStatus);
        command.Parameters.AddWithValue("@role", (int)user.Role);

        if (user.Role.Equals(UserRole.Vendor))
            command.Parameters.AddWithValue("@status", (int)AccountStatus.Pending);
        else
            command.Parameters.AddWithValue("@status", (int)AccountStatus.Active);

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
    public async Task<Admin> GetAdminByUsernameOrEmailAsync(string usernameOrEmail)
        {
        using var connection = _databaseContext.CreateConnection();
            
                await connection.OpenAsync();

                const string query = @"
                SELECT a.id, a.person_id, a.username, a.password, a.profile_image, a.update_date, a.status, a.role
                FROM admin a
                JOIN person p ON a.person_id = p.id
                WHERE a.username = @usernameOrEmail OR p.email = @usernameOrEmail";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@usernameOrEmail", usernameOrEmail);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
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
                    }
             
            return null;
        }



}
