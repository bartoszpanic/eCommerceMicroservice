using MongoDB.Driver;
using Product.Application;

namespace Product.Infrastructure;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Shared.Product> _products;

    public ProductRepository(MongoDbService mongoDbService)
    {
        if (mongoDbService.Database == null)
        {
            throw new ArgumentNullException(nameof(mongoDbService.Database), "Database cannot be null.");
        }

        _products = mongoDbService.Database.GetCollection<Shared.Product>("products")
                   ?? throw new ArgumentNullException(nameof(_products), "Products collection cannot be null.");
    }

    public async Task<List<Shared.Product>> GetProductsAsync()
    {
        try
        {
            return await _products.Find(FilterDefinition<Shared.Product>.Empty)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            // Log the exception (use a logging library like Serilog or NLog)
            throw new ApplicationException("An error occurred while retrieving products.", ex);
        }
    }
}
