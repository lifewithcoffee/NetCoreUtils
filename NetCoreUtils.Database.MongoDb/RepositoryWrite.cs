using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.Database.MongoDb
{
    public interface IRepositoryWrite<TDoc> where TDoc : MongoDocBase
    {
        void DeleteMany(Expression<Func<TDoc, bool>> where, IClientSessionHandle session = null);
        Task DeleteManyAsync(Expression<Func<TDoc, bool>> where, IClientSessionHandle session = null);
        void DeleteOne(Expression<Func<TDoc, bool>> where, IClientSessionHandle session = null);
        Task DeleteOneAsync(Expression<Func<TDoc, bool>> where, IClientSessionHandle session = null);
        void InsertMany(ICollection<TDoc> documents, IClientSessionHandle session = null);
        Task InsertManyAsync(ICollection<TDoc> documents, IClientSessionHandle session = null);
        void InsertOne(TDoc document, IClientSessionHandle session = null);
        Task InsertOneAsync(TDoc document, IClientSessionHandle session = null);
        void Replace(Expression<Func<TDoc, bool>> where, TDoc doc, IClientSessionHandle session = null);
        Task ReplaceAsync(Expression<Func<TDoc, bool>> where, TDoc doc, IClientSessionHandle session = null);
        void Update(Expression<Func<TDoc, bool>> where, UpdateDefinition<TDoc> update, IClientSessionHandle session = null);
        Task UpdateAsync(Expression<Func<TDoc, bool>> where, UpdateDefinition<TDoc> update, IClientSessionHandle session = null);
    }

    /// <summary>
    /// This class is left here just for reference. It is not recommended to
    /// use it since it seems not to make sense to wrap a Repository class for
    /// writing.
    /// 
    /// Direct injecting IMongoCollection<TDoc> should be used instead.
    /// </summary>
    public class RepositoryWrite<TDoc> : IRepositoryWrite<TDoc> where TDoc : MongoDocBase
    {
        private IMongoCollection<TDoc> _collection;

        public RepositoryWrite(IMongoDbConnection dbconn)
        {
            _collection = dbconn.MongoDatabase.GetCollection<TDoc>(typeof(TDoc).Name);
        }

        public void InsertOne(TDoc document, IClientSessionHandle session = null)
        {
            if (session == null)
                _collection.InsertOne(document);
            else
                _collection.InsertOne(session, document);
        }

        public async Task InsertOneAsync(TDoc document, IClientSessionHandle session = null)
        {
            if (session == null)
                await _collection.InsertOneAsync(document);
            else
                await _collection.InsertOneAsync(session, document);
        }

        public void InsertMany(ICollection<TDoc> documents, IClientSessionHandle session = null)
        {
            if (session == null)
                _collection.InsertMany(documents);
            else
                _collection.InsertMany(session, documents);
        }

        public async Task InsertManyAsync(ICollection<TDoc> documents, IClientSessionHandle session = null)
        {
            if (session == null)
                await _collection.InsertManyAsync(documents);
            else
                await _collection.InsertManyAsync(session, documents);
        }

        public void Replace(Expression<Func<TDoc, bool>> where, TDoc doc, IClientSessionHandle session = null)
        {
            if (session == null)
                _collection.FindOneAndReplace(where, doc);
            else
                _collection.FindOneAndReplace(session, where, doc);
        }

        public async Task ReplaceAsync(Expression<Func<TDoc, bool>> where, TDoc doc, IClientSessionHandle session = null)
        {
            if (session == null)
                await _collection.FindOneAndReplaceAsync(where, doc);
            else
                await _collection.FindOneAndReplaceAsync(session, where, doc);
        }

        public void Update(Expression<Func<TDoc, bool>> where, UpdateDefinition<TDoc> update, IClientSessionHandle session = null)
        {
            if (session == null)
                _collection.FindOneAndUpdate(where, update);
            else
                _collection.FindOneAndUpdate(session, where, update);
        }

        public async Task UpdateAsync(Expression<Func<TDoc, bool>> where, UpdateDefinition<TDoc> update, IClientSessionHandle session = null)
        {
            if (session == null)
                await _collection.FindOneAndUpdateAsync(where, update);
            else
                await _collection.FindOneAndUpdateAsync(session, where, update);
        }

        public void DeleteOne(Expression<Func<TDoc, bool>> where, IClientSessionHandle session = null)
        {
            if (session == null)
                _collection.DeleteOne(where);
            else
                _collection.DeleteOne(session, where);

        }

        public async Task DeleteOneAsync(Expression<Func<TDoc, bool>> where, IClientSessionHandle session = null)
        {
            if (session == null)
                await _collection.DeleteOneAsync(where);
            else
                await _collection.DeleteOneAsync(session, where);
        }

        public void DeleteMany(Expression<Func<TDoc, bool>> where, IClientSessionHandle session = null)
        {
            if (session == null)
                _collection.DeleteMany(where);
            else
                _collection.DeleteMany(session, where);
        }

        public async Task DeleteManyAsync(Expression<Func<TDoc, bool>> where, IClientSessionHandle session = null)
        {
            if (session == null)
                await _collection.DeleteManyAsync(where);
            else
                await _collection.DeleteManyAsync(session, where);
        }
    }
}
