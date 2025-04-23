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

    public NpgsqlConnection CreateConnection()
    {
        //return new NpgsqlConnection(_configuration.DefaultConnection);

        //return new("User Id=postgres.mglzwmdcowngkudaxqmr;Password=Cosmiticsdotnet123+;Server=aws-0-eu-west-2.pooler.supabase.com;Port=6543;Database=postgres");
        //return new NpgsqlConnection(_configuration.GetConnectionString(DataBase));
        return new("User Id=postgres;Password=admin;Server=localhost;Port=5432;Database=lumora;");
        //return new(_configuration.DefaultConnection);

    }
}
