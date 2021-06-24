
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetCoreUtils.Database
{
    public interface IRepositoryRead<TEntity> where TEntity : class
    {
        TEntity Get(int? id);
        Task<TEntity> GetAsync(int? id);

        // Return IQueryable to use QueryableExtensions methods like Load(), Include() etc.
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where);
        IQueryable<TEntity> QueryAll();

        bool Exist(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity GetByIdNoTracking(int? id);
        Task<TEntity> GetByIdNoTrackingAsync(int? id);
        IQueryable<TEntity> GetAllNoTracking();
        IQueryable<TEntity> GetManyNoTracking(Expression<Func<TEntity, bool>> where);

        IQueryable<TEntity> GetManyLocalFirst(Expression<Func<TEntity, bool>> where);
    }

    public class RepositoryRead<TEntity> : IRepositoryRead<TEntity> where TEntity : class
    {
        private IUnitOfWork _unitOfWork;
        private DbSet<TEntity> dbSet;

        public RepositoryRead(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this.dbSet = unitOfWork.Context.Set<TEntity>();
        }

        public virtual bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Any<TEntity>(predicate);
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AnyAsync<TEntity>(predicate);
        }

        public virtual IQueryable<TEntity> QueryAll()
        {
            return dbSet;
        }

        public virtual TEntity Get(int? id)
        {
            if(id == null)
                return null;
            else
                return dbSet.Find(id);
        }

        public virtual async Task<TEntity> GetAsync(int? id)
        {
            if(id == null)
                return null;
            else
                return await dbSet.FindAsync(id);
        }


        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            // Don't use "where.Compile(), otherwise when do "ToList()", such an exception will throw out: 
            // "There is already an open DataReader associated with this Command which must be closed first"
            return dbSet.Where(where);
        }

        public virtual IQueryable<TEntity> GetAllNoTracking()
        {
            return dbSet.AsNoTracking();
        }

        // from: https://stackoverflow.com/questions/34967116/how-to-combine-find-and-asnotracking
        public virtual TEntity GetByIdNoTracking(int? id)
        {
            var entity = this.Get(id);
            if(entity != null)
                _unitOfWork.Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public virtual async Task<TEntity> GetByIdNoTrackingAsync(int? id)
        {
            var entity = await this.GetAsync(id);
            if(entity != null)
                _unitOfWork.Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        /// <summary>
        /// WARNING: be careful to use this method, if not VERY sure, always use "GetMany" to load from database
        ///          rather than using this "GetManyLocalFirst" to return from what's already in memory.
        /// 
        /// The original idea of this method is: get data immediately after adding data, the data should be better
        /// get directly from memory (aka. local)
        /// 
        /// However, if call this method twice with a writing between the 2 calls, then the second call
        /// will only return the dataset from the loaded first call, i.e. the just written new data will not
        /// be included in the return collection.
        /// </summary>
        /// <returns>See the return comment of <see cref="QueryAll()"/></returns>
        public virtual IQueryable<TEntity> GetManyLocalFirst(Expression<Func<TEntity, bool>> where)
        {
            return dbSet.FindLocalFirst(where);
        }

        public virtual IQueryable<TEntity> GetManyNoTracking(Expression<Func<TEntity, bool>> where)
        {
            return dbSet.AsNoTracking().Where(where);
        }
    }
}