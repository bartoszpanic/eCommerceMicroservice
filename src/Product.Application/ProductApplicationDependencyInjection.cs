using Microsoft.Extensions.DependencyInjection;
using Product.Application.Services;

namespace Product.Application;

public static class ProductApplicationDependencyInjection
{
    public static IServiceCollection AddProductApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
