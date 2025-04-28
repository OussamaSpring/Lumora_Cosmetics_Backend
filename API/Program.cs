using API;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Persistence.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add CORS - Fixed policy name consistency
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp",  // Changed to match usage below
        policy => policy
            .WithOrigins("http://localhost:3000", "https://localhost:3000") // Added HTTPS
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()); // Added if using cookies/auth
});

// Configuration
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(nameof(JwtSettings)));

// Services
builder.Services.AddScoped<IDbContext, DbContext>();
builder.Services.AddScoped<ITokenProvider, JwtTokenProvider>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddRepositories();
builder.Services.AddServices();





builder.Services.AddScoped<IProductService, ProductService>();
// Controllers - Removed duplicate registration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseRouting();

// CORS - Fixed policy name
app.UseCors("ReactApp"); // Now matches the defined policy


// Authentication/Authorization
app.UseAuthentication(); // Added this missing middleware
app.UseAuthorization();

app.MapControllers();

app.Run();