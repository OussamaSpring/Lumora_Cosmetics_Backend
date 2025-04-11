using Application.Interfaces;
using Application.Services;
using Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Validate and get connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Database connection string 'DefaultConnection' not found.");

// Add services to the container
builder.Services.AddControllers();

// Register services with DI
builder.Services.AddScoped<IProductRepository>(_ => new ProductRepository(connectionString));
builder.Services.AddScoped<IProductService, ProductService>();

// Add health checks (optional but recommended)
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString);  // Requires AspNetCore.HealthChecks.NpgSql NuGet package

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Better error messages in development
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapHealthChecks("/health");  // Health check endpoint

app.Run();