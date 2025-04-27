using Microsoft.Extensions.Configuration;

using MongoDB.Driver;


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
            _database.CreateCollection("products");
        }
    }
}
