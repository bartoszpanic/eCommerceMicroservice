using FluentResults;
using MongoDB.Bson;
using MongoDB.Driver;
using Product.Application;
using Product.Application.Dtos;
using Product.Application.Logger;

namespace Product.Infrastructure;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Shared.Product> _products;
    private readonly ILoggerService<ProductRepository> _loggerService;

    public ProductRepository(MongoDbService mongoDbService, ILoggerService<ProductRepository> loggerService)
    {
        if (mongoDbService.Database == null)
        {
            throw new ArgumentNullException(nameof(mongoDbService.Database), "Database cannot be null.");
        }

        _products = mongoDbService.Database.GetCollection<Shared.Product>("products")
                   ?? throw new ArgumentNullException(nameof(_products), "Products collection cannot be null.");
        _loggerService = loggerService;
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
                isDeleted = false,
                Description = product.Description
            };

            _products.InsertOne(newProduct);
            var id = await Task.FromResult(newProduct.Id.ToString());
            _loggerService.LogInfo($"Product created with ID: {id}");
            return Result.Ok(id);

        }
        catch (Exception ex)
        {
            var message = $"An error occurred while creating the product. {ex.Message}";
            _loggerService.LogError(message, ex);
            return Result.Fail(new Error("An error occurred while creating the product.").CausedBy(ex));
        }
    }

    public async Task<Result<Shared.Product>> GetProductByIdAsync(string id)
    {
        try
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                var message = $"Invalid product ID format: {id}";
                _loggerService.LogError(message);
                return Result.Fail(new Error(message));
            }

            var filter = Builders<Shared.Product>.Filter.Eq(p => p.Id, objectId.ToString());
            var product = await _products.Find(filter).FirstOrDefaultAsync();

            if (product == null)
            {
                var message = $"Product with ID {id} not found.";
                _loggerService.LogError(message);
                return Result.Fail(new Error(message));
            }

            return Result.Ok(product);
        }
        catch (Exception ex)
        {
            var message = $"An error occurred while retrieving the product with ID {id}. {ex.Message}";
            _loggerService.LogError(message, ex);
            return Result.Fail(new Error(message).CausedBy(ex));
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

    public async Task<Result> SoftDeleteProductAsync(string id)
    {
        try
        {
            var filter = Builders<Shared.Product>.Filter.Eq(p => p.Id, id);
            var update = Builders<Shared.Product>.Update.Set(p => p.isDeleted, true);
            var result = await _products.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
            {
                var message = $"Product with ID {id} not found or already deleted.";
                _loggerService.LogError(message);
                return Result.Fail(new Error(message));
            }

            return Result.Ok();
        }
        catch (Exception ex)
        {
            var message = $"An error occurred while soft deleting the product with ID {id}. {ex.Message}";
            _loggerService.LogError(message, ex);
            return Result.Fail(new Error("An error occurred while soft deleting the product.").CausedBy(ex));
        }
    }

}
