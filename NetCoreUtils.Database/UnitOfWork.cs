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
    public interface ICommittable: IDisposable
    {
        bool Commit();
        Task<bool> CommitAsync();
    }

    public interface IUnitOfWork<TDbContext>: ICommittable where TDbContext : DbContext
    {
        TDbContext Context { get; }
    }

    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private TDbContext _context;
        private readonly ILogger _logger;

        public TDbContext Context { get { return _context; } }

        public UnitOfWork(TDbContext context, ILogger<ICommittable> logger)
        {
            _context = context;
            _logger = logger;

            _logger.LogTrace("Initializing UnitOfWork with system default setting");
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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
