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
        return new("Host=dpg-d06l4tpr0fns73foldf0-a.oregon-postgres.render.com;Database=lumora;Username=lumora_user;Password=o1MiXXaEmDFQHR4DbPQU38hxXD3r1O66;Port=5432;SSL Mode=Require;Trust Server Certificate=true;");
        //return new(_configuration.DefaultConnection);

    }
}
