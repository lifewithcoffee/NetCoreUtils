using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCoreUtils.Database
{
    public interface IRepositoryBase<TEntity, TDbContext>
        : ICommittable
        , IRepositoryRead<TEntity>
        , IRepositoryWrite<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        void EnableQueryTracking(bool enabled);
        TDbContext Context { get; }
    }

    /// <summary>
    /// originated from (but changed quite a lot):
    /// http://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    /// https://github.com/MarlabsInc/webapi-angularjs-spa/blob/28bea19b3267aeed1768920b0d77be329b0278a5/source/ResourceMetadata/ResourceMetadata.Data/Infrastructure/RepositoryBase.cs
    /// </summary>
    abstract public class RepositoryBase<TEntity, TDbContext>
        : IRepositoryBase<TEntity, TDbContext>
        , ICommittable
        where TEntity : class 
        where TDbContext : DbContext
    {
        private readonly IUnitOfWork<TDbContext> _unitOfWork;
        private readonly ILogger _logger;
        private readonly DbSet<TEntity> dbSet;

        private readonly RepositoryRead<TEntity, TDbContext> repoReader;
        private readonly RepositoryWrite<TEntity, TDbContext> repoWriter;

        protected IUnitOfWork<TDbContext> UnitOfWork { get { return _unitOfWork; } }
        protected ILogger Logger { get { return _logger; } }
        protected DbSet<TEntity> DbSet { get { return dbSet; } }

        public TDbContext Context { get {return _unitOfWork.Context;} }

        public RepositoryBase(IUnitOfWork<TDbContext> unitOfWork, ILogger<RepositoryBase<TEntity, TDbContext>> logger)
        {
            _unitOfWork = unitOfWork;
            dbSet = unitOfWork.Context.Set<TEntity>();

            repoReader = new RepositoryRead<TEntity, TDbContext>(unitOfWork);
            repoWriter = new RepositoryWrite<TEntity, TDbContext>(unitOfWork);

            _logger = logger;
        }

        public void EnableQueryTracking(bool enabled)
        {
            if(enabled)
                _unitOfWork.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            else
                _unitOfWork.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            return repoReader.Exist(predicate);
        }

        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await repoReader.ExistAsync(predicate);
        }

        public TEntity GetById(int? id)
        {
            return repoReader.GetById(id);
        }

        // from: https://stackoverflow.com/questions/34967116/how-to-combine-find-and-asnotracking
        public TEntity GetByIdNoTracking(int? id)
        {
            return repoReader.GetByIdNoTracking(id);
        }

        public async Task<TEntity> GetByIdAsync(int? id)
        {
            return await repoReader.GetByIdAsync(id);
        }

        public async Task<TEntity> GetByIdNoTrackingAsync(int? id)
        {
            return await repoReader.GetByIdNoTrackingAsync(id);
        }

        public virtual TEntity Add(TEntity entity)
        {
            return repoWriter.Add(entity); // NOTIC: see comment of RepositoryWrite.Add() for "Reason of not implementing AddAsync"
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            repoWriter.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            repoWriter.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            repoWriter.UpdateRange(entities);
        }

        public virtual void Remove(TEntity entity)  // there is no DbSet.RemoveAsync() available
        {
            repoWriter.Remove(entity);
        }

        public virtual void Remove(Expression<Func<TEntity, bool>> where)
        {
            repoWriter.Remove(where);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            repoWriter.RemoveRange(entities);
        }

        /// <returns>Return IQueryable to use QueryableExtensions methods like Load(), Include() etc. </returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return repoReader.GetAll();
        }

        public IQueryable<TEntity> GetAllNoTracking()
        {
            return repoReader.GetAllNoTracking();
        }
        
        /// <returns>See the return comment of <see cref="GetAll()"/></returns>
        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            return repoReader.GetMany(where);
        }

        public IQueryable<TEntity> GetManyNoTracking(Expression<Func<TEntity, bool>> where)
        {
            return repoReader.GetManyNoTracking(where);
        }

        public virtual IQueryable<TEntity> GetManyLocalFirst(Expression<Func<TEntity, bool>> where)
        {
            return repoReader.GetManyLocalFirst(where); // NOTICE: see RepositoryBaseRead.GetManyLocalFirst()'s comment
        }

        public bool Commit()
        {
            return repoWriter.Commit();
        }

        public async Task<bool> CommitAsync()
        {
            return await repoWriter.CommitAsync();
        }
    }
}