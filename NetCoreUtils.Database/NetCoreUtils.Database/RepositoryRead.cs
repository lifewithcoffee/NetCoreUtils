
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
    public interface IRepositoryRead<TEntity> where TEntity : class
    {
        TEntity Get(int? id);
        Task<TEntity> GetAsync(int? id);

        // Return IQueryable to use QueryableExtensions methods like Load(), Include() etc.
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where);
        IQueryable<TEntity> QueryAll();

        bool Exist(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate);
    }

    public class RepositoryRead<TEntity> : IRepositoryRead<TEntity> where TEntity : class
    {
        private IUnitOfWork _unitOfWork;
        private DbSet<TEntity> dbSet;

        public RepositoryRead(IUnitOfWork unitOfWork)
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

        public virtual IQueryable<TEntity> QueryAll()
        {
            return dbSet;
        }

        public virtual TEntity Get(int? id)
        {
            if(id == null)
                return null;
            else
                return dbSet.Find(id);
        }

        public virtual async Task<TEntity> GetAsync(int? id)
        {
            if(id == null)
                return null;
            else
                return await dbSet.FindAsync(id);
        }


        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            // Don't use "where.Compile(), otherwise when do "ToList()", such an exception will throw out: 
            // "There is already an open DataReader associated with this Command which must be closed first"
            return dbSet.Where(where);
        }
    }
}