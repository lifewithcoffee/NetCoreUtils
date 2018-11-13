using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.Database
{
    public interface ICommittable
    {
        bool Commit();
        Task<bool> CommitAsync();
    }

    public interface IUnitOfWork<TDbContext>: ICommittable where TDbContext : DbContext
    {
        TDbContext Context { get; }
        void EnableQueryTracking(bool enabled);
        void RejectAllChanges();
    }

    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private TDbContext _context;
        private readonly ILogger _logger;

        public TDbContext Context { get { return _context; } }

        public UnitOfWork(TDbContext context, ILogger<UnitOfWork<TDbContext>> logger)
        {
            _context = context;
            _logger = logger;

            _logger.LogTrace("Initializing UnitOfWork with system default setting");
        }

        public void EnableQueryTracking(bool enabled)
        {
            if(enabled)
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            else
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<bool> CommitAsync()
        {
            bool result = false;
            try
            {
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return result;
        }

        public void RejectAllChanges()
        {
            var changedEntries = _context.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged);
            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified: // todo: all apply "entry.State == EntityState.Unchanged;" instead?
                    case EntityState.Deleted:
                        entry.Reload(); 
                        break;
                }
            }
        }

        public bool Commit()
        {
            bool result = false;
            try
            {
                _context.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return result;
        }
    }
}
