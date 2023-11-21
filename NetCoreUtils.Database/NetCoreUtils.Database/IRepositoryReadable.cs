
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCoreUtils.Database
{
    public interface IRepositoryReadable<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(int? id);

        // Return IQueryable to use QueryableExtensions methods like Load(), Include() etc.
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where);

        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetByIdAsync(int? id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where);

        IQueryable<TEntity> GetManyLocalFirst(Expression<Func<TEntity, bool>> where);
        IQueryable<TEntity> QueryAll();
    }
}