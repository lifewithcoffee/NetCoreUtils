
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetCoreUtils.Database
{
    /**
     * - Do not use Aysnc methods in writing operations, see the comment in
     *   Add() and AddRange() implementations for details
     *   
     * - Need to declare an impl in the application project:
     * 
     *     public class RepositoryWriter<TEntity>
     *       : RepositoryWrite<TEntity, ApplicationDbContext>
     *       where TEntity : class
     *     {
     *       public RepositoryWriter(IUnitOfWork<ApplicationDbContext> unitOfWork)
     *           : base(unitOfWork)
     *       { }
     *     }
     */
    public interface IRepositoryWritable<TEntity> : ICommittable where TEntity : class
    {
        TEntity Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void Remove(Expression<Func<TEntity, bool>> where);
        void RemoveAll();
        void RemoveRange(IEnumerable<TEntity> entities);
    }

}