
using Npgsql;
using Infrastructure.Persistence;
using Domain.Entities.AccountRelated;
using Domain.Enums.enAccount;
using Application.Interfaces;


namespace Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        public async Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();

                const string query = @"
                SELECT u.id, u.person_id, u.username, u.password, u.profile_image, u.update_date, u.status, u.role
                FROM ""user"" u
                JOIN person p ON u.person_id = p.id
                WHERE u.username = @usernameOrEmail OR p.email = @usernameOrEmail";

                using (var command = new NpgsqlCommand(query, connection))
                {
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
                            Profile_Image_URL = reader.GetValue(4) != DBNull.Value ? reader.GetString(4) : null,
                            UpdateDate = reader.GetDateTime(5),
                            AccountStatus = (AccountStatus)reader.GetInt16(6),
                            Role = (UserRole)reader.GetInt16(7)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<Admin> GetAdminByUsernameOrEmailAsync(string usernameOrEmail)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();

                const string query = @"
                SELECT a.id, a.person_id, a.username, a.password, a.profile_image, a.update_date, a.status, a.role
                FROM admin a
                JOIN person p ON a.person_id = p.id
                WHERE a.username = @usernameOrEmail OR p.email = @usernameOrEmail";

                using (var command = new NpgsqlCommand(query, connection))
                {
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
                }
            }
            return null;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();

                const string query = "SELECT COUNT(1) FROM \"user\" WHERE username = @username";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    var count = (long)await command.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();

                const string query = "SELECT COUNT(1) FROM person WHERE email = @email";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    var count = (long)await command.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

        public async Task<Guid> CreatePersonAsync(Person person)
        {
            // When first signup we only store these basic information of person (firstname, lastname, email)
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();

                const string query = @"
                INSERT INTO person (first_name, last_name, email)
                VALUES (@firstName, @lastName, @email)
                RETURNING id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@firstName", person.FirstName);
                    command.Parameters.AddWithValue("@lastName", person.LastName);
                    command.Parameters.AddWithValue("@email", person.Email);

                    return (Guid)await command.ExecuteScalarAsync();
                }
            }
        }

        public async Task<Guid> CreateUserAsync(User user)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();

                const string query = @"
                INSERT INTO ""user"" (person_id, username, password, status, role)
                VALUES (@personId, @username, @password, @status, @role) 
                RETURNING id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@personId", user.PersonId);
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@role", (int)user.Role);

                    if(user.Role.Equals(UserRole.Vendor))
                        command.Parameters.AddWithValue("@status", (int)AccountStatus.Pending);
                    else
                        command.Parameters.AddWithValue("@status", (int)AccountStatus.Active);


                    return (Guid)await command.ExecuteScalarAsync();
                }
            }
        }

        public async Task<string> GenerateUniqueUsername(string firstName)
        {
                string baseUsername = firstName.ToLower();
                Random random = new Random();
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
    }
}
