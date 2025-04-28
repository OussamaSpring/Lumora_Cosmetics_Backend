using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Persistence.Repositories;

namespace API;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IVarianteTypeRepository, VariantTypeRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IImageService, ImageService>();

        services.AddScoped<IUserAuthentication, UserAuthentication>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<IVariantTypeService, VariantTypeService>();
        services.AddScoped<ISearchService, SearchService>();

        return services;
    }
}