using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCoreUtils.Database
{
    /**
     * Need to declare an impl in the application project:
     *
     *  public class RepositoryBase<TEntity>
     *      : RepositoryBase<TEntity, ApplicationDbContext>
     *      where TEntity : class
     *  {
     *      public RepositoryBase(
     *          IUnitOfWork<ApplicationDbContext> unitOfWork,
     *          IRepositoryRead<TEntity, ApplicationDbContext> repoReader,
     *          IRepositoryWrite<TEntity, ApplicationDbContext> repoWriter
     *      ) : base(unitOfWork, repoReader, repoWriter)
     *      { }
     *  }
     */
    public interface IRepository<TEntity>
        : ICommittable
        , IRepositoryRead<TEntity>
        , IRepositoryWrite<TEntity>
        where TEntity : class
    { }

    /**
     * For DI registration:
     * services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
     */
    public interface IRepository<TEntity, TDbContext>
        : IRepository<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
    }

    /// <summary>
    /// originated from (but changed quite a lot):
    /// http://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    /// https://github.com/MarlabsInc/webapi-angularjs-spa/blob/28bea19b3267aeed1768920b0d77be329b0278a5/source/ResourceMetadata/ResourceMetadata.Data/Infrastructure/RepositoryBase.cs
    /// </summary>
    public class Repository<TEntity, TDbContext>
        : IRepository<TEntity, TDbContext>
        where TEntity : class 
        where TDbContext : DbContext
    {
        private readonly IRepositoryRead<TEntity, TDbContext> _repoReader;
        private readonly IRepositoryWrite<TEntity, TDbContext> _repoWriter;

        public Repository(
            IRepositoryRead<TEntity, TDbContext> repoReader,
            IRepositoryWrite<TEntity, TDbContext> repoWriter
        ){

            _repoReader = repoReader;
            _repoWriter = repoWriter;
        }

        public bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            return _repoReader.Exist(predicate);
        }

        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repoReader.ExistAsync(predicate);
        }

        public TEntity Get(int? id)
        {
            return _repoReader.Get(id);
        }

        public async Task<TEntity> GetAsync(int? id)
        {
            return await _repoReader.GetAsync(id);
        }

        public virtual TEntity Add(TEntity entity)
        {
            return _repoWriter.Add(entity); // NOTIC: see comment of RepositoryWrite.Add() for "Reason of not implementing AddAsync"
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _repoWriter.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _repoWriter.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _repoWriter.UpdateRange(entities);
        }

        public virtual void Remove(TEntity entity)  
        {
            _repoWriter.Remove(entity);
        }

        public virtual void Remove(Expression<Func<TEntity, bool>> where)
        {
            _repoWriter.Remove(where);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _repoWriter.RemoveRange(entities);
        }

        public virtual IQueryable<TEntity> QueryAll()
        {
            return _repoReader.QueryAll();
        }
        
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            return _repoReader.Query(where);
        }

        public bool Commit()
        {
            return _repoWriter.Commit();
        }

        public async Task<bool> CommitAsync()
        {
            return await _repoWriter.CommitAsync();
        }
    }
}