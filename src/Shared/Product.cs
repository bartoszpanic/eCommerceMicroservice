using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared;

public record Product
{
    [BsonId]
    [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; init; } = Guid.NewGuid();

    [BsonElement("product_name")]
    public string ProductName { get; set; } = string.Empty;
    [BsonElement("product_price")]
    public decimal Price { get; set; }
}
