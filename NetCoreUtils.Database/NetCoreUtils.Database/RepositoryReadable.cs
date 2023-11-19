
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetCoreUtils.Database.MultiTenancy;

namespace NetCoreUtils.Database
{
    /**
     * Declared as abstract to prevent it to be instantiated.
     * Use <see cref="RepositoryReadonly{TEntity}"> to instantiate a readonly instance
     */
    abstract public class RepositoryReadable<TEntity> : IRepositoryReadable<TEntity> where TEntity : class
    {
        protected IUnitOfWork _unitOfWork;
        protected DbSet<TEntity> _dbSet;

        public RepositoryReadable(IUnitOfWork unitOfWork)
        {
            this._dbSet = unitOfWork.Context.Set<TEntity>();
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync<TEntity>(predicate);
        }

        public virtual async Task<TEntity> GetAsync(int? id)
        {
            if (id == null)
                return null;
            else
                return await _dbSet.FindAsync(id);
        }


        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            /**
             * Don't use "where.Compile(), otherwise when do "ToList()", such
             * an exception will throw out:
             * "There is already an open DataReader associated with this Command which must be closed first"
             */
            return _dbSet.Where(where);
        }

        public virtual IQueryable<TEntity> QueryAll()
        {
            return _dbSet;
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(int? id)
        {
            /**
             * See also: https://stackoverflow.com/questions/34967116/how-to-combine-find-and-asnotracking
             * if (entity != null)
             *    _unitOfWork.Context.Entry(entity).State = EntityState.Detached;
             */
            var entity = await this.GetAsync(id);
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
            return _dbSet.FindLocalFirst(where);
        }

        public virtual async Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where)
        {
            return await _dbSet.Where(where).ToListAsync();
        }
    }
}