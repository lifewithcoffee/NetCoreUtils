using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.Database.MongoDb
{
    public class RepositoryWrite<TDoc> where TDoc : MongoDocBase
    {
        private IMongoCollection<TDoc> _collection;

        public RepositoryWrite(IMongoDbConnection dbconn)
        {
            _collection = dbconn.MongoDatabase.GetCollection<TDoc>(typeof(TDoc).Name);
        }

        public void InsertOne(TDoc document)
        {
            _collection.InsertOne(document);
        }

        public async Task InsertOneAsync(TDoc document)
        {
            await _collection.InsertOneAsync(document);
        }

        public void InsertMany(ICollection<TDoc> documents)
        {
            _collection.InsertMany(documents);
        }

        public async Task InsertManyAsync(ICollection<TDoc> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void Replace(Expression<Func<TDoc, bool>> where,TDoc doc)
        {
            _collection.FindOneAndReplace(where, doc);
        }
        
        public async Task ReplaceAsync(Expression<Func<TDoc, bool>> where,TDoc doc)
        {
            await _collection.FindOneAndReplaceAsync(where, doc);
        }

        public async Task ReplaceAsync(IClientSessionHandle session, Expression<Func<TDoc, bool>> where,TDoc doc)
        {
            await _collection.FindOneAndReplaceAsync(session, where, doc);
        }

        public void Update(Expression<Func<TDoc, bool>> where, UpdateDefinition<TDoc> update)
        {
            _collection.FindOneAndUpdate(where, update);
        }

        public async Task UpdateAsync(Expression<Func<TDoc, bool>> where, UpdateDefinition<TDoc> update)
        {
            await _collection.FindOneAndUpdateAsync(where, update);
        }

        public void DeleteOne(Expression<Func<TDoc, bool>> where)
        {
            _collection.DeleteOne(where);
        }

        public async Task DeleteOneAsync(Expression<Func<TDoc, bool>> where)
        {
            await _collection.DeleteOneAsync(where);
        }

        public void DeleteMany(Expression<Func<TDoc, bool>> where)
        {
            _collection.DeleteMany(where);
        }

        public async Task DeleteManyAsync(Expression<Func<TDoc, bool>> where)
        {
            await _collection.DeleteManyAsync(where);
        }
    }
}
