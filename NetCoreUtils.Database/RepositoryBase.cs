using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCoreUtils.Database
{
    public interface IRepositoryWrite<TEntity>
        where TEntity : class
    {
        TEntity Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> where);
        void DeleteRange(IEnumerable<TEntity> entities);
    }

    public interface IRepositoryBase<TEntity>
        : ICommittable
        , IRepositoryRead<TEntity>
        , IRepositoryWrite<TEntity>
        where TEntity : class
    {
        void EnableQueryTracking(bool enabled);
    }

    /// <summary>
    /// originated from (but changed quite a lot):
    /// http://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    /// https://github.com/MarlabsInc/webapi-angularjs-spa/blob/28bea19b3267aeed1768920b0d77be329b0278a5/source/ResourceMetadata/ResourceMetadata.Data/Infrastructure/RepositoryBase.cs
    /// </summary>
    abstract public class RepositoryBase<TEntity, TDbContext>
        : IRepositoryBase<TEntity>, ICommittable
        where TEntity : class 
        where TDbContext : DbContext
    {
        private readonly IUnitOfWork<TDbContext> _unitOfWork;
        private readonly ILogger _logger;
        private readonly DbSet<TEntity> dbSet;

        private readonly RepositoryBaseRead<TEntity, TDbContext> repoRead;

        protected IUnitOfWork<TDbContext> UnitOfWork { get { return _unitOfWork; } }
        protected ILogger Logger { get { return _logger; } }
        protected DbSet<TEntity> DbSet { get { return dbSet; } }

        public TDbContext Context { get {return _unitOfWork.Context;} }

        public RepositoryBase(IUnitOfWork<TDbContext> unitOfWork, ILogger<RepositoryBase<TEntity, TDbContext>> logger)
        {
            _unitOfWork = unitOfWork;
            dbSet = unitOfWork.Context.Set<TEntity>();

            repoRead = new RepositoryBaseRead<TEntity, TDbContext>(unitOfWork);

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
            return repoRead.Exist(predicate);
        }

        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await repoRead.ExistAsync(predicate);
        }

        public TEntity GetById(int? id)
        {
            return repoRead.GetById(id);
        }

        // from: https://stackoverflow.com/questions/34967116/how-to-combine-find-and-asnotracking
        public TEntity GetByIdNoTracking(int? id)
        {
            return repoRead.GetByIdNoTracking(id);
        }

        public async Task<TEntity> GetByIdAsync(int? id)
        {
            return await repoRead.GetByIdAsync(id);
        }

        public async Task<TEntity> GetByIdNoTrackingAsync(int? id)
        {
            return await repoRead.GetByIdNoTrackingAsync(id);
        }

        /// <summary> Reason of not implementing AddAsync:
        /// According to: http://stackoverflow.com/questions/42034282/are-there-dbset-updateasync-and-removeasync-in-net-core
        /// DbSet.AddAsync() should not be used.
        /// 
        /// Quote:
        ///
        /// AddAsync however, only begins tracking an entity but won't actually send any changes 
        /// to the database until you call SaveChanges or SaveChangesAsync. You shouldn't really 
        /// be using this method unless you know what you're doing. The reason the async version 
        /// of this method exists is explained in the docs:
        /// 
        /// This method is async only to allow special value generators, such as the one used by
        /// 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        /// to access the database asynchronously. For all other cases the non async method should
        /// be used.
        /// 
        /// The same reason is also applied to Remove and Update methods.
        /// </summary>
        public virtual TEntity Add(TEntity entity)
        {
            dbSet.Add(entity);
            return entity;
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            //old impl in EF
            //dbSet.Attach(entity);
            //_unitOfWork.Context.Entry(entity).State = EntityState.Modified;

            // new impl in EF core
            dbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            dbSet.UpdateRange(entities);
        }

        public virtual void Delete(TEntity entity)  // there is no DbSet.RemoveAsync() available
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            IEnumerable<TEntity> objects = dbSet.Where<TEntity>(where).AsEnumerable();
            dbSet.RemoveRange(objects);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }

        /// <returns>Return IQueryable to use QueryableExtensions methods like Load(), Include() etc. </returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return repoRead.GetAll();
        }

        public IQueryable<TEntity> GetAllNoTracking()
        {
            return repoRead.GetAllNoTracking();
        }
        
        /// <returns>See the return comment of <see cref="GetAll()"/></returns>
        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            return repoRead.GetMany(where);
        }

        public IQueryable<TEntity> GetManyNoTracking(Expression<Func<TEntity, bool>> where)
        {
            return repoRead.GetManyNoTracking(where);
        }

        public virtual IQueryable<TEntity> GetManyLocalFirst(Expression<Func<TEntity, bool>> where)
        {
            return repoRead.GetManyLocalFirst(where); // NOTICE: see RepositoryBaseRead.GetManyLocalFirst()'s comment
        }

        public bool Commit()
        {
            return this._unitOfWork.Commit();
        }

        public async Task<bool> CommitAsync()
        {
            return await this._unitOfWork.CommitAsync();
        }

        public void Dispose()
        {
            this._unitOfWork.Dispose();
        }
    }
}