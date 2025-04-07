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
        var client = new MongoClient(_configuration.GetConnectionString("MongoDb"));
        _database = client.GetDatabase(_configuration["DatabaseName"]);
    }

    public IMongoDatabase? Database => _database;
}
