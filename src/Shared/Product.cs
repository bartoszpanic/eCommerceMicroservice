﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared;

public record Product
{
    [BsonId]
    [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; }

    [BsonElement("product_name")]
    public string ProductName { get; set; } = string.Empty;
    [BsonElement("product_price")]
    public decimal Price { get; set; }
    [BsonElement("isDeleted")]
    public bool isDeleted { get; set; }
    [BsonElement("created_at")]
    public DateTimeOffset CreatedAt { get; init; }
    public string Description { get; set; } = string.Empty;
}
