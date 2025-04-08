using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Product.Application;
using Product.Application.Logger;
using Product.Infrastructure.Logger;

namespace Product.Infrastructure;

public static class ProductInfrastructureDependencyInjection
{
    public static IServiceCollection AddProductInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(typeof(ILoggerService<>), typeof(LoggerService<>));
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            loggingBuilder.AddNLog(configuration);
        });

        services.AddSingleton<MongoDbService>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
