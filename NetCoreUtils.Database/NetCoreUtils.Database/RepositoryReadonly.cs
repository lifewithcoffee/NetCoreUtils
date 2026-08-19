using Microsoft.EntityFrameworkCore;

namespace NetCoreUtils.Database
{
    public interface IRepositoryReadonly<TEntity> : IRepositoryReadable<TEntity> where TEntity : class { }

    /// <summary>
    /// RepositoryReadonly has special configurations to improve the performance
    /// </summary>
    public class RepositoryReadonly<TEntity> : RepositoryReadable<TEntity>, IRepositoryReadonly<TEntity> where TEntity : class
    {
        public RepositoryReadonly(DbContext ctx) : base(ctx) 
        {
            ctx.ChangeTracker.AutoDetectChangesEnabled = false;
            ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;      // can be verified by: context.ChangeTracker.Entries().Count()
            ctx.ChangeTracker.LazyLoadingEnabled = false;    // disable proxy creation
        }
    }
}