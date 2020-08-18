using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.Database.MongoDb
{
    public interface IRepositoryWrite<TDoc> : IMongoRepository<TDoc> where TDoc : MongoDoc
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
    public class RepositoryWrite<TDoc> : RepositoryBase<TDoc>, IRepositoryWrite<TDoc> where TDoc : MongoDoc
    {
        public RepositoryWrite(IMongoDbConnection conn):base(conn) { }

        public void InsertOne(TDoc document, IClientSessionHandle session = null)
        {
            if (session == null)
                Collection.InsertOne(document);
            else
                Collection.InsertOne(session, document);
        }

        public async Task InsertOneAsync(TDoc document, IClientSessionHandle session = null)
        {
            if (session == null)
                await Collection.InsertOneAsync(document);
            else
                await Collection.InsertOneAsync(session, document);
        }

        public void InsertMany(ICollection<TDoc> documents, IClientSessionHandle session = null)
        {
            if (session == null)
                Collection.InsertMany(documents);
            else
                Collection.InsertMany(session, documents);
        }

        public async Task InsertManyAsync(ICollection<TDoc> documents, IClientSessionHandle session = null)
        {
            if (session == null)
                await Collection.InsertManyAsync(documents);
            else
                await Collection.InsertManyAsync(session, documents);
        }

        public void Replace(Expression<Func<TDoc, bool>> where, TDoc doc, IClientSessionHandle session = null)
        {
            if (session == null)
                Collection.FindOneAndReplace(where, doc);
            else
                Collection.FindOneAndReplace(session, where, doc);
        }

        public async Task ReplaceAsync(Expression<Func<TDoc, bool>> where, TDoc doc, IClientSessionHandle session = null)
        {
            if (session == null)
                await Collection.FindOneAndReplaceAsync(where, doc);
            else
                await Collection.FindOneAndReplaceAsync(session, where, doc);
        }

        public void Update(Expression<Func<TDoc, bool>> where, UpdateDefinition<TDoc> update, IClientSessionHandle session = null)
        {
            if (session == null)
                Collection.FindOneAndUpdate(where, update);
            else
                Collection.FindOneAndUpdate(session, where, update);
        }

        public async Task UpdateAsync(Expression<Func<TDoc, bool>> where, UpdateDefinition<TDoc> update, IClientSessionHandle session = null)
        {
            if (session == null)
                await Collection.FindOneAndUpdateAsync(where, update);
            else
                await Collection.FindOneAndUpdateAsync(session, where, update);
        }

        public void DeleteOne(Expression<Func<TDoc, bool>> where, IClientSessionHandle session = null)
        {
            if (session == null)
                Collection.DeleteOne(where);
            else
                Collection.DeleteOne(session, where);

        }

        public async Task DeleteOneAsync(Expression<Func<TDoc, bool>> where, IClientSessionHandle session = null)
        {
            if (session == null)
                await Collection.DeleteOneAsync(where);
            else
                await Collection.DeleteOneAsync(session, where);
        }

        public void DeleteMany(Expression<Func<TDoc, bool>> where, IClientSessionHandle session = null)
        {
            if (session == null)
                Collection.DeleteMany(where);
            else
                Collection.DeleteMany(session, where);
        }

        public async Task DeleteManyAsync(Expression<Func<TDoc, bool>> where, IClientSessionHandle session = null)
        {
            if (session == null)
                await Collection.DeleteManyAsync(where);
            else
                await Collection.DeleteManyAsync(session, where);
        }
    }
}
