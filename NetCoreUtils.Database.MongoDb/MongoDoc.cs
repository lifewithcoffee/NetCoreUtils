using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace NetCoreUtils.Database.MongoDb
{

    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionAttribute : Attribute
    {
        string collectionName;

        public CollectionAttribute(string collectionName)
        {
            this.collectionName = collectionName;
        }

        public string CollectionName { get => collectionName; }
    }


    public class MongoDoc
    {
        [BsonId]
        public ObjectId _id { get; set; }
    }
}
