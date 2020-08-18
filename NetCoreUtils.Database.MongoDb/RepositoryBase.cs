using MongoDB.Driver;

namespace NetCoreUtils.Database.MongoDb
{
    public class RepositoryBase<TDoc>
    {
        protected IMongoDatabase _database;

        private IMongoCollection<TDoc> _collection;
        public IMongoCollection<TDoc> Collection { get => _collection; }

        private string _collectionName;
        public string CollectionName
        {
            get { return _collectionName; }
            set
            {
                _collection = _database.GetCollection<TDoc>(value);
                _collectionName = value;
            }
        }

        public RepositoryBase(IMongoDbConnection conn)
        {
            _database = conn.MongoDatabase;
        }
    }
}
