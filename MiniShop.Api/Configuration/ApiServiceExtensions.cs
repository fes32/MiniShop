using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.Application.Interfaces;
using MiniShop.Application.Services;
using MiniShop.Infrastructure.Persistence;
using MiniShop.Infrastructure.Repositories;

namespace MiniShop.Api.Configuration;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddMiniShopApiServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}
