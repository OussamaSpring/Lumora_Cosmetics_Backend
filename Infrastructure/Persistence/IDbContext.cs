using Npgsql;

namespace Infrastructure.Persistence;

public interface IDbContext
{
    NpgsqlConnection CreateConnection();
}
