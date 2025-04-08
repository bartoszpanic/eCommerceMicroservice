using FluentResults;
using MongoDB.Driver;
using Product.Application;
using Product.Application.Dtos;

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

    public async Task<Result<string>> CreateProductAsync(ProductDto product)
    {
        try
        {
            var newProduct = new Shared.Product
            {
                ProductName = product.ProductName,
                Price = product.Price,
                CreatedAt = DateTimeOffset.UtcNow,
                isDeleted = false
            };

            _products.InsertOne(newProduct);
            var id = await Task.FromResult(newProduct.Id.ToString());
            return Result.Ok(id);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("An error occurred while creating the product.").CausedBy(ex));
        }
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
