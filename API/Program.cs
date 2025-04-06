namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            // Initialize the database context with the connection string
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            Infrastructure.Persistence.DatabaseContext.Initialize(connectionString);


            builder.Services.AddControllers();




            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
