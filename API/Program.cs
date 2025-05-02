using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Persistence.Repositories;
using Microsoft.OpenApi.Any;

namespace API;

// Add CORS - Fixed policy name consistency
builder.Services.AddCors(options =>
{
    options.AddPolicy("All",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()); // Added if using cookies/auth)
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



       
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddControllers();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

app.UseRouting();

// CORS - Fixed policy name
app.UseCors("All"); // Now matches the defined policy


        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
