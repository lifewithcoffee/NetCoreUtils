using MongoDB.Driver;

namespace NetCoreUtils.Database.MongoDb
{
    public interface IMongoRepository<TDoc>
    {
        string CollectionName { get; }
        IMongoCollection<TDoc> Collection { get; }
    }
}
