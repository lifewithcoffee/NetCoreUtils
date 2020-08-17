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
    public interface IRepositoryRead<TDoc> where TDoc : MongoDocBase
    {
        bool Exist(Expression<Func<TDoc, bool>> where);
        Task<bool> ExistAsync(Expression<Func<TDoc, bool>> where);
        List<TDoc> Find(Expression<Func<TDoc, bool>> where);
        Task<List<TDoc>> FindAsync(Expression<Func<TDoc, bool>> where);
        TDoc Get(string id);
        Task<TDoc> GetAsync(string id);
        IMongoQueryable<TDoc> Query(Expression<Func<TDoc, bool>> where);
        IMongoQueryable<TDoc> QueryAll();
    }

    public class RepositoryRead<TEntity> : IRepositoryRead<TEntity> where TEntity : MongoDocBase
    {
        private IMongoCollection<TEntity> _collection;

        public RepositoryRead(IMongoDatabase db)
        {
            _collection = db.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public bool Exist(Expression<Func<TEntity, bool>> where)
        {
            return _collection.Find(where).Any();
        }

        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> where)
        {
            var cursor = await _collection.FindAsync(where);
            return await cursor.AnyAsync();
        }

        public TEntity Get(string id)
        {
            var objectId = new ObjectId(id);
            return _collection.Find(d => d.Id.Equals(objectId)).SingleOrDefault();

            // or:
            // var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
            // return _collection.Find(filter).SingleOrDefault();
        }

        public async Task<TEntity> GetAsync(string id)
        {
            var cursor = await _collection.FindAsync<TEntity>(d => d.Id.Equals(new ObjectId(id)));
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> where)
        {
            var cursor = await _collection.FindAsync<TEntity>(where);
            return await cursor.ToListAsync();
        }

        public List<TEntity> Find(Expression<Func<TEntity, bool>> where)
        {
            return _collection.Find<TEntity>(where).ToList();
        }

        public IMongoQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            return _collection.AsQueryable<TEntity>().Where(where);
        }

        public IMongoQueryable<TEntity> QueryAll()
        {
            return _collection.AsQueryable<TEntity>();
        }
    }
}
