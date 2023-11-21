using Microsoft.EntityFrameworkCore;

namespace NetCoreUtils.Database
{
    public interface IRepositoryReadonly<TEntity> : IRepositoryReadable<TEntity> where TEntity : class { }

    public class RepositoryReadonly<TEntity> : RepositoryReadable<TEntity>, IRepositoryReadonly<TEntity> where TEntity : class
    {
        public RepositoryReadonly(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {
            /**
             * Another proposed approach to have a readonly repository (may need to upgrade ef core to 7.0):
             * 
             * unitOfWork.Context.Configuration.AutoDetectChangesEnabled = false;       // better to use this for batch updating, use with DetectChanges()
             * unitOfWork.Context.Configuration.ProxyCreationEnabled = false;
             */
            //unitOfWork.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;      // verify by: context.ChangeTracker.Entries().Count()
        }
    }
}