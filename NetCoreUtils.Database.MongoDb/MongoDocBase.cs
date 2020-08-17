using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace NetCoreUtils.Database.MongoDb
{
    public class MongoDocBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}
