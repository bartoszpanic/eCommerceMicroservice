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
                isDeleted = false,
                Description = product.Description
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

    public async Task<Result> SoftDeleteProductAsync(string id)
    {
        try
        {
            var filter = Builders<Shared.Product>.Filter.Eq(p => p.Id, id);
            var update = Builders<Shared.Product>.Update.Set(p => p.isDeleted, true);
            var result = await _products.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
            {
                return Result.Fail(new Error("Product not found or already deleted."));
            }

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("An error occurred while soft deleting the product.").CausedBy(ex));
            throw;
        }
    }

}
