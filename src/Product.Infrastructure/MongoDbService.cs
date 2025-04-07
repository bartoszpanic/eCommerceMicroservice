using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Shared;

namespace Product.Infrastructure;

public class MongoDbService
{
    private readonly IConfiguration _configuration;
    private readonly IMongoDatabase _database;

    public MongoDbService(IConfiguration configuration)
    {
        _configuration = configuration;

        var connectionString = configuration.GetConnectionString("DbConnection");
        var mongoUrl = new MongoUrl(connectionString);
        var mongoClient = new MongoClient(mongoUrl);

        _database = mongoClient.GetDatabase("eCommerceDb");

        EnsureDatabaseInitialized();
    }

    public IMongoDatabase? Database => _database;

    private void EnsureDatabaseInitialized()
    {
        var collections = _database.ListCollections().ToList();
        if (!collections.Any())
        {
            AddProductsRecords();
        }
    }

    private void AddProductsRecords()
    {
        _database.CreateCollection("products");
        var collection = _database.GetCollection<Shared.Product>("products");
        var products = new List<Shared.Product>
        {
            new Shared.Product { ProductName = "Product 1", Price = 10.0m },
            new Shared.Product { ProductName = "Product 2", Price = 20.0m },
            new Shared.Product { ProductName = "Product 3", Price = 30.0m }
        };

        collection.InsertMany(products);
    }
}
