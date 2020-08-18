using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;

namespace NetCoreUtils.Database.MongoDb
{
    public interface IMongoRepository<TDoc>
    {
        string CollectionName { get; set; }
        IMongoCollection<TDoc> Collection { get; }
    }

    public class MongoDoc
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime CreatedAt => Id.CreationTime;
    }
}
