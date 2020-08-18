using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace NetCoreUtils.Database.MongoDb
{

    public class MongoDoc
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime CreatedAt => Id.CreationTime;
    }
}
