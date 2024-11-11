using Microsoft.EntityFrameworkCore;

namespace NetCoreUtils.Database
{
    public interface IRepositoryReadonly<TEntity> : IRepositoryReadable<TEntity> where TEntity : class { }

    public class RepositoryReadonly<TEntity> : RepositoryReadable<TEntity>, IRepositoryReadonly<TEntity> where TEntity : class
    {
        public RepositoryReadonly(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {
            unitOfWork.Context.ChangeTracker.AutoDetectChangesEnabled = false;
            unitOfWork.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;      // verify by: context.ChangeTracker.Entries().Count()
            unitOfWork.Context.ChangeTracker.LazyLoadingEnabled = false;    // disable proxy creation
        }
    }
}