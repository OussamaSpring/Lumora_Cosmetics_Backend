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
                WHERE u.username = @usernameOrEmail OR
                      p.email = @usernameOrEmail";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@usernameOrEmail", usernameOrEmail);

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

        object result = await command.ExecuteScalarAsync();
        if (result == null || result == DBNull.Value)
        {
            return false;
        }

        return true;
    }

    
    public async Task<Guid> CreateUserAsync(User user)
    {
        using var connection = _databaseContext.CreateConnection();
        await connection.OpenAsync();

        const string query1 = @"
                INSERT INTO person (first_name, last_name, email)
                VALUES (@firstName, @lastName, @email) 
                RETURNING id";

        using var command1 = new NpgsqlCommand(query1, connection);

        command1.Parameters.AddWithValue("@firstName", user.FirstName);
        command1.Parameters.AddWithValue("@lastName", user.LastName);
        command1.Parameters.AddWithValue("@email", user.Email);

        var personId = (Guid)await command1.ExecuteScalarAsync();

        user.Id = personId;

        const string query2 = @"
                INSERT INTO ""user"" (id, person_id, username, password, status, role)
                VALUES (@userId, @personId, @username, @password, @status, @role) 
                RETURNING id";

        await connection.OpenAsync();
        using var command = new NpgsqlCommand(query2, connection);
        command.Parameters.AddWithValue("@userId", user.Id);
        command.Parameters.AddWithValue("@personId", user.Id);
        command.Parameters.AddWithValue("@username", user.Username);
        command.Parameters.AddWithValue("@password", user.Password);
        command.Parameters.AddWithValue("@role", (int)user.Role);
        command.Parameters.AddWithValue("@status", (int)user.AccountStatus);


        return (Guid)await command.ExecuteScalarAsync();
    }

    public async Task<string> GenerateUniqueUsername(string firstName)
    {
        string baseUsername = firstName.ToLower();
        Random random = new();
        string username;
        bool exists;
        byte attempts = 0;

        do
        {
            int randomNumber = random.Next(100, 999);
            username = $"{baseUsername}{randomNumber}";

            exists = await UsernameExistsAsync(username);
            attempts++;


            if (attempts > 10) // Prevent infinite loop
            {
                username = $"{baseUsername}{DateTime.Now:fff}"; // Fallback
                exists = await UsernameExistsAsync(username);
                if (exists)
                {
                    throw new Exception("Failed to generate unique username");
                }
            }


        } while (exists);

        return username;
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
