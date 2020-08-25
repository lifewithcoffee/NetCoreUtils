using System;
using MongoDB.Driver;

namespace NetCoreUtils.Database.MongoDb
{
    public class RepositoryBase<TDoc> : IMongoRepository<TDoc>
    {
        protected IMongoDatabase _database;

        private IMongoCollection<TDoc> _collection;
        public IMongoCollection<TDoc> Collection { get => _collection; }

        private string _collectionName;
        public string CollectionName
        {
            get { return _collectionName; }
        }

        public RepositoryBase(IMongoDbConnection conn)
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
