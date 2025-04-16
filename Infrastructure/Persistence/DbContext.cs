using API;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Infrastructure.Persistence;

public class DbContext : IDbContext
{
    private readonly DbConfiguration _configuration;

    public DbContext(IOptions<DbConfiguration> configuration)
    {
        _configuration = configuration.Value;
    }

    public NpgsqlConnection CreateConnection() =>
        new("User Id=postgres.mglzwmdcowngkudaxqmr;Password=Cosmiticsdotnet123+;Server=aws-0-eu-west-2.pooler.supabase.com;Port=6543;Database=postgres");

    internal object Set<T>()
    {
        throw new NotImplementedException();
    }
    //return new NpgsqlConnection(_configuration.DefaultConnection);
    //return new NpgsqlConnection(_configuration.GetConnectionString(DataBase));
}
