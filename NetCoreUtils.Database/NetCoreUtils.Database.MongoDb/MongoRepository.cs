using MongoDB.Driver;
using System;

namespace NetCoreUtils.Database.MongoDb
{
    public interface IMongoRepository<TDoc>
    {
        string CollectionName { get; }
        IMongoCollection<TDoc> Collection { get; }
    }

    public class MongoRepository<TDoc> : IMongoRepository<TDoc>
    {
        protected IMongoDatabase _database;

        private IMongoCollection<TDoc> _collection;
        public IMongoCollection<TDoc> Collection { get => _collection; }

        private string _collectionName;
        public string CollectionName
        {
            get { return _collectionName; }
        }

        public MongoRepository(IMongoDbConnection conn)
        {
            _database = conn.MongoDatabase;

            var attr = (CollectionAttribute)Attribute.GetCustomAttribute(typeof(TDoc), typeof(CollectionAttribute));
            if (attr != null)
            {
                _collectionName = attr.CollectionName;
            }
            else
            {
                _collectionName = typeof(TDoc).Name.ToLower() + "_collection";
            }

            _collection = _database.GetCollection<TDoc>(_collectionName);
        }
    }

}
