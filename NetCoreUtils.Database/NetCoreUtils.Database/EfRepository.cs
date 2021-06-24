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

    public class EfRepository<TEntity> : Repository<TEntity> , IEfRepository<TEntity> where TEntity : class 
    {
        private IEfRepositoryRead<TEntity> _efRepoRead;

        public EfRepository(
            IRepositoryRead<TEntity> repoReader,
            IRepositoryWrite<TEntity> repoWriter,
            IEfRepositoryRead<TEntity> efRepoRead
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
