using API;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Persistence.Repositories;
using Domain.Entities.ProductRelated;


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

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserAuthentication, UserAuthentication>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();

builder.Services.AddScoped<IVariantTypeRepository, VariantTypeRepository>();
builder.Services.AddScoped<IVariantTypeService, VariantTypeService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // Disable HTTPS redirection in development
    // app.UseHttpsRedirection(); 
}

app.UseRouting();

// CORS - Fixed policy name
app.UseCors("ReactApp"); // Now matches the defined policy

// Authentication/Authorization
app.UseAuthentication(); // Added this missing middleware
app.UseAuthorization();

app.MapControllers();

app.Run();