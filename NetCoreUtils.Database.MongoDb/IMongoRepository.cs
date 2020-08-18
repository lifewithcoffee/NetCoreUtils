using MongoDB.Driver;

namespace NetCoreUtils.Database.MongoDb
{
    public interface IMongoRepository<TDoc>
    {
        string CollectionName { get; set; }
        IMongoCollection<TDoc> Collection { get; }
    }
}
