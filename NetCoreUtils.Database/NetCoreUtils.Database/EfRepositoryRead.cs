using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.Database
{
    public interface IEfRepositoryRead<TEntity>
        : IRepositoryRead<TEntity>
        where TEntity : class
    {
        TEntity GetByIdNoTracking(int? id);
        Task<TEntity> GetByIdNoTrackingAsync(int? id);
        IQueryable<TEntity> GetAllNoTracking();
        IQueryable<TEntity> GetManyNoTracking(Expression<Func<TEntity, bool>> where);

        IQueryable<TEntity> GetManyLocalFirst(Expression<Func<TEntity, bool>> where);
    }

    public interface IEfRepositoryRead<TEntity, TDbContext>
        : IEfRepositoryRead<TEntity>
        , IRepositoryRead<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext
    { }

    class EfRepositoryRead<TEntity, TDbContext>
        : IEfRepositoryRead<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext
    {
        private IUnitOfWork<TDbContext> _unitOfWork;
        private DbSet<TEntity> dbSet;
        private IRepositoryRead<TEntity, TDbContext> _repoReader;

        public EfRepositoryRead(IUnitOfWork<TDbContext> unitOfWork, IRepositoryRead<TEntity, TDbContext> repoReader)
        {
            this._unitOfWork = unitOfWork;
            this.dbSet = unitOfWork.Context.Set<TEntity>();
            this._repoReader = repoReader;
        }

        #region delegate for IRepositoryRead<TEntity, TDbContext
        public virtual bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            return _repoReader.Exist(predicate);
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repoReader.ExistAsync(predicate);
        }

        public virtual TEntity Get(int? id)
        {
            return _repoReader.Get(id);
        }

        public virtual async Task<TEntity> GetAsync(int? id)
        {
            return await _repoReader.GetAsync(id);
        }

        public virtual IQueryable<TEntity> QueryAll()
        {
            return _repoReader.QueryAll();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            return _repoReader.Query(where);
        }
        #endregion

        public virtual IQueryable<TEntity> GetAllNoTracking()
        {
            return dbSet.AsNoTracking();
        }

        // from: https://stackoverflow.com/questions/34967116/how-to-combine-find-and-asnotracking
        public virtual TEntity GetByIdNoTracking(int? id)
        {
            var entity = _repoReader.Get(id);
            _unitOfWork.Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public virtual async Task<TEntity> GetByIdNoTrackingAsync(int? id)
        {
            var entity = await _repoReader.GetAsync(id);
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
