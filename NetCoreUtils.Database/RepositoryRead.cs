
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetCoreUtils.Database
{
    /**
     * Need to declare an impl in the application project:
     * 
     * public class RepositoryReader<TEntity>
     *   : RepositoryReader<TEntity, ApplicationDbContext>
     *   where TEntity : class
     * {
     *   public RepositoryReader(IUnitOfWork<ApplicationDbContext> unitOfWork)
     *       : base(unitOfWork)
     *   { }
     * }
     */
    public interface IRepositoryRead<TEntity>
        where TEntity : class
    {
        TEntity GetById(int? id);
        Task<TEntity> GetByIdAsync(int? id);

        // Return IQueryable to use QueryableExtensions methods like Load(), Include() etc.
        IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where);
        IQueryable<TEntity> GetAll();

        bool Exist(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate);
    }
    
   /**
    * For DI registration:
    * services.AddScoped(typeof(IRepositoryRead<,>), typeof(RepositoryRead<,>));
    */
    public interface IRepositoryRead<TEntity, TDbContext>
        : IRepositoryRead<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    { }

    public class RepositoryRead<TEntity, TDbContext>
        : IRepositoryRead<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext
    {
        private IUnitOfWork<TDbContext> _unitOfWork;
        private DbSet<TEntity> dbSet;

        public RepositoryRead(IUnitOfWork<TDbContext> unitOfWork)
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

        public virtual IQueryable<TEntity> GetAll()
        {
            return dbSet;
        }

        public virtual TEntity GetById(int? id)
        {
            if(id == null)
                return null;
            else
                return dbSet.Find(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(int? id)
        {
            if(id == null)
                return null;
            else
                return await dbSet.FindAsync(id);
        }


        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            // Don't use "where.Compile(), otherwise when do "ToList()", such an exception will throw out: 
            // "There is already an open DataReader associated with this Command which must be closed first"
            return dbSet.Where(where);
        }
    }
}