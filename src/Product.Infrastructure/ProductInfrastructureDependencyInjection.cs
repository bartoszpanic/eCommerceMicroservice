using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application;

namespace Product.Infrastructure;

public static class ProductInfrastructureDependencyInjection
{
    public static IServiceCollection AddProductInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MongoDbService>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
