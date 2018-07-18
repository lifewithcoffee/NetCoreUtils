using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCoreUtils.Database
{
    /// <summary>
    /// Make up the missing methods for EntityFramework Core DbSet object.
    /// </summary>
    static public class EfCoreExt
    {
        /// <summary>
        /// from: http://stackoverflow.com/questions/29030472/dbset-doesnt-have-a-find-method-in-ef7
        /// This method has not been tested yet, so it's not used in production, just leave here as reference.
        /// </summary>
        public static TEntity Find<TEntity>(this DbSet<TEntity> set, params object[] keyValues) where TEntity : class
        {
            var context = ((IInfrastructure<IServiceProvider>)set).GetService<DbContext>();

            var entityType = context.Model.FindEntityType(typeof(TEntity));
            var key = entityType.FindPrimaryKey();

            var entries = context.ChangeTracker.Entries<TEntity>();

            var i = 0;
            foreach (var property in key.Properties)
            {
                entries = entries.Where(e => e.Property(property.Name).CurrentValue == keyValues[i]);
                i++;
            }

            var entry = entries.FirstOrDefault();
            if (entry != null)
            {
                // Return the local object if it exists.
                return entry.Entity;
            }

            // TODO: Build the real LINQ Expression
            // set.Where(x => x.Id == keyValues[0]);
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var query = set.Where((Expression<Func<TEntity, bool>>)
                Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, "Id"),
                        Expression.Constant(keyValues[0])),
                    parameter));

            // Look in the database
            return query.FirstOrDefault();
        }

        /// <summary>
        /// from: http://stackoverflow.com/questions/33819159/is-there-a-dbsettentity-local-equivalent-in-entity-framework-7
        /// </summary>
        public static ObservableCollection<TEntity> GetLocal<TEntity>(this DbSet<TEntity> set) where TEntity : class
        {
            var context = set.GetService<DbContext>();
            var data = context.ChangeTracker.Entries<TEntity>().Select(e => e.Entity);
            var collection = new ObservableCollection<TEntity>(data);

            collection.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    context.AddRange(e.NewItems.Cast<TEntity>());
                }

                if (e.OldItems != null)
                {
                    context.RemoveRange(e.OldItems.Cast<TEntity>());
                }
            };

            return collection;
        }
    }

    /// <summary>
    /// from: http://blog.cincura.net/233451-using-entity-frameworks-find-method-with-predicate/
    /// </summary>
    static public class EfExt
    {
        public static IQueryable<T> FindPredicateFromLocalAndDb<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> predicate) where T : class
        {
            var local = dbSet.GetLocal().Where(predicate.Compile()); // query 'Local' to see if data has been loaded
            if (local.Any())
                return local.AsQueryable();
            else
                return dbSet.Where(predicate); // load data from the database
        }

        /// <returns>Not sure how to asynchronously return a IQueryable, so use IEnumerable instead</returns>
        public static async Task<IEnumerable<T>> FindPredicateAsync<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> predicate) where T : class
        {
            var local = dbSet.GetLocal().Where(predicate.Compile());
            if (local.Any())
                return local;
            else
                return await dbSet.Where(predicate).ToListAsync().ConfigureAwait(false);
        }
    }

    public interface IRepositoryBase<TEntity, TDbContext> : ICommittable
        where TEntity : class
        where TDbContext : DbContext
    {
        TEntity GetById(int? id);
        Task<TEntity> GetByIdAsync(int? id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where);
        IQueryable<TEntity> GetManyLocalFirst(Expression<Func<TEntity, bool>> where);

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
        protected DbSet<TEntity> DbSet { get { return DbSet; } }

        public RepositoryBase(IUnitOfWork<TDbContext> unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;

            dbSet = unitOfWork.Context.Set<TEntity>();
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
            {
                return null;
            }
            else
            {
                return dbSet.Find(id); // Note: the Find() is an extension method on the top of this file.
            }
        }

        public async Task<TEntity> GetByIdAsync(int? id)
        {
            if(id == null)
            {
                return null;
            }
            else
            {
                return await dbSet.FindAsync(id);
            }
        }

        /// <summary> Reason of not implementing AddAsync:
        /// 
        /// According to: http://stackoverflow.com/questions/42034282/are-there-dbset-updateasync-and-removeasync-in-net-core
        /// DbSet.AddAsync() should not be used:
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
        /// 
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
            // old impl in EF
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
            foreach (TEntity obj in objects)
            {
                this.Delete(obj);
            }
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

        /// <returns>See the return comment of <see cref="GetAll()"/></returns>
        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            // Don't use "where.Compile(), otherwise when do "ToList()", such an exception will throw out: 
            // "There is already an open DataReader associated with this Command which must be closed first"
            return dbSet.Where(where);
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
            return dbSet.FindPredicateFromLocalAndDb(where);
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