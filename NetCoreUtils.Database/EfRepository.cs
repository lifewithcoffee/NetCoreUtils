using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.Database
{
    public interface IEfRepository<TEntity>
        : IRepository<TEntity>
        , IEfRepositoryRead<TEntity>
        where TEntity : class
    { }

    public interface IEfRepository<TEntity, TDbContext>
        : IEfRepository<TEntity>
        , IRepository<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext
    { }

    public class EfRepository<TEntity, TDbContext>
        : Repository<TEntity, TDbContext>
        , IEfRepository<TEntity, TDbContext>
        where TEntity : class 
        where TDbContext : DbContext
    {
        private IEfRepositoryRead<TEntity, TDbContext> _efRepoRead;

        public EfRepository(
            IRepositoryRead<TEntity, TDbContext> repoReader,
            IRepositoryWrite<TEntity, TDbContext> repoWriter,
            IEfRepositoryRead<TEntity, TDbContext> efRepoRead
            ):base(repoReader,repoWriter)
        {
            _efRepoRead = efRepoRead;
        }

        public TEntity GetByIdNoTracking(int? id)
        {
            return _efRepoRead.GetByIdNoTracking(id);
        }

        public async Task<TEntity> GetByIdNoTrackingAsync(int? id)
        {
            return await _efRepoRead.GetByIdNoTrackingAsync(id);
        }

        public IQueryable<TEntity> GetAllNoTracking()
        {
            return _efRepoRead.GetAllNoTracking();
        }

        public IQueryable<TEntity> GetManyNoTracking(Expression<Func<TEntity, bool>> where)
        {
            return _efRepoRead.GetManyNoTracking(where);
        }

        public virtual IQueryable<TEntity> GetManyLocalFirst(Expression<Func<TEntity, bool>> where)
        {
            return _efRepoRead.GetManyLocalFirst(where); // NOTICE: see RepositoryBaseRead.GetManyLocalFirst()'s comment
        }
    }
}
