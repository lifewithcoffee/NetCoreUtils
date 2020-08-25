using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.Database.MongoDb
{
    public interface IMongoDocReader<TDoc> : IMongoRepository<TDoc> where TDoc : MongoDoc
    {
        bool Exist(Expression<Func<TDoc, bool>> where);
        Task<bool> ExistAsync(Expression<Func<TDoc, bool>> where);
        List<TDoc> Find(Expression<Func<TDoc, bool>> where);
        Task<List<TDoc>> FindAsync(Expression<Func<TDoc, bool>> where);
        TDoc Find(string id);
        Task<TDoc> FindAsync(string id);
        IMongoQueryable<TDoc> Query(Expression<Func<TDoc, bool>> where);
        IMongoQueryable<TDoc> Query();
    }

    public class MongoDocReader<TDoc> : MongoRepository<TDoc>, IMongoDocReader<TDoc> where TDoc : MongoDoc
    {
        public MongoDocReader(IMongoDbConnection conn) : base(conn) { }

        public bool Exist(Expression<Func<TDoc, bool>> where)
        {
            return Collection.Find(where).Any();
        }

        public async Task<bool> ExistAsync(Expression<Func<TDoc, bool>> where)
        {
            var cursor = await Collection.FindAsync(where);
            return await cursor.AnyAsync();
        }

        public TDoc Find(string id)
        {
            return Collection.Find(d => d.Id.Equals(new ObjectId(id))).SingleOrDefault();

            // or:
            //return Collection.Find(d => d.Id.Equals(ObjectId.Parse(id))).SingleOrDefault();

            // or:
            //return Collection.Find(d => d.Id == id).SingleOrDefault();

            // or:
            //var filter = Builders<TDoc>.Filter.Eq("_id", id);
            //return Collection.Find(filter).SingleOrDefault();
        }

        public async Task<TDoc> FindAsync(string id)
        {
            var cursor = await Collection.FindAsync<TDoc>(d => d.Id.Equals(new ObjectId(id)));
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<List<TDoc>> FindAsync(Expression<Func<TDoc, bool>> where)
        {
            var cursor = await Collection.FindAsync<TDoc>(where);
            return await cursor.ToListAsync();
        }

        public List<TDoc> Find(Expression<Func<TDoc, bool>> where)
        {
            return Collection.Find<TDoc>(where).ToList();
        }

        public IMongoQueryable<TDoc> Query(Expression<Func<TDoc, bool>> where)
        {
            return Collection.AsQueryable<TDoc>().Where(where);
        }

        public IMongoQueryable<TDoc> Query()
        {
            return Collection.AsQueryable<TDoc>();
        }
    }
}
