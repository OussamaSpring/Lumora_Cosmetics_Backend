using Npgsql;

namespace Infrastructure.Persistence
{
    public class DatabaseContext
    {
        private static string _connectionString;

        public static void Initialize(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
