using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCoreUtils.Database
{
    public interface IRepositoryBase<TEntity, TDbContext> : ICommittable
        where TEntity : class
        where TDbContext : DbContext
    {
        TDbContext Context { get; }

        void EnableQueryTracking(bool enabled);

        TEntity GetById(int? id);
        Task<TEntity> GetByIdAsync(int? id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where);
        IQueryable<TEntity> GetManyLocalFirst(Expression<Func<TEntity, bool>> where);

        TEntity GetByIdNoTracking(int? id);
        Task<TEntity> GetByIdNoTrackingAsync(int? id);
        IQueryable<TEntity> GetAllNoTracking();
        IQueryable<TEntity> GetManyNoTracking(Expression<Func<TEntity, bool>> where);

        bool Exist(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> where);
        void DeleteRange(IEnumerable<TEntity> entities);
    }

    /// <summary>
    /// originated from (but changed quite a lot):
    /// http://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    /// https://github.com/MarlabsInc/webapi-angularjs-spa/blob/28bea19b3267aeed1768920b0d77be329b0278a5/source/ResourceMetadata/ResourceMetadata.Data/Infrastructure/RepositoryBase.cs
    /// </summary>
    abstract public class RepositoryBase<TEntity, TDbContext>
        : IRepositoryBase<TEntity, TDbContext>, ICommittable
        where TEntity : class 
        where TDbContext : DbContext
    {
        private readonly IUnitOfWork<TDbContext> _unitOfWork;
        private readonly ILogger _logger;
        private readonly DbSet<TEntity> dbSet;

        protected IUnitOfWork<TDbContext> UnitOfWork { get { return _unitOfWork; } }
        protected ILogger Logger { get { return _logger; } }
        protected DbSet<TEntity> DbSet { get { return dbSet; } }

        public TDbContext Context { get {return _unitOfWork.Context;} }

        public RepositoryBase(IUnitOfWork<TDbContext> unitOfWork, ILogger<RepositoryBase<TEntity, TDbContext>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;

            dbSet = unitOfWork.Context.Set<TEntity>();
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
            return dbSet.Any<TEntity>(predicate);
        }

        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AnyAsync<TEntity>(predicate);
        }

        public TEntity GetById(int? id)
        {
            if(id == null)
                return null;
            else
                return dbSet.Find(id);
        }

        // from: https://stackoverflow.com/questions/34967116/how-to-combine-find-and-asnotracking
        public TEntity GetByIdNoTracking(int? id)
        {
            var entity = this.GetById(id);
            Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<TEntity> GetByIdAsync(int? id)
        {
            if(id == null)
                return null;
            else
                return await dbSet.FindAsync(id);
        }

        public async Task<TEntity> GetByIdNoTrackingAsync(int? id)
        {
            var entity = await GetByIdAsync(id);
            Context.Entry(entity).State = EntityState.Detached;
            return entity;
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
            return dbSet;
        }

        public IQueryable<TEntity> GetAllNoTracking()
        {
            return dbSet.AsNoTracking();
        }
        
        /// <returns>See the return comment of <see cref="GetAll()"/></returns>
        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            // Don't use "where.Compile(), otherwise when do "ToList()", such an exception will throw out: 
            // "There is already an open DataReader associated with this Command which must be closed first"
            return dbSet.Where(where);
        }

        public IQueryable<TEntity> GetManyNoTracking(Expression<Func<TEntity, bool>> where)
        {
            return dbSet.AsNoTracking().Where(where);
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
        /// <returns>See the return comment of <see cref="GetAll()"/></returns>
        public virtual IQueryable<TEntity> GetManyLocalFirst(Expression<Func<TEntity, bool>> where)
        {
            return dbSet.FindLocalFirst(where);
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