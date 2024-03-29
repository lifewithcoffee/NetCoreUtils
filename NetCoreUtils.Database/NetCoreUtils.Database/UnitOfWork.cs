﻿using System.Runtime.ExceptionServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetCoreUtils.Database.MultiTenant;

namespace NetCoreUtils.Database
{
    public interface ICommittable
    {
        bool Commit();
        Task<bool> CommitAsync();
    }

    public interface IUnitOfWork : ICommittable
    {
        DbContext Context { get; }

        void RejectAllChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _context;

        private ITenantProvider _tenantProvider;
        private readonly ILogger _logger;

        public DbContext Context { get { return _context; } }

        public UnitOfWork(DbContext context, ITenantProvider tenantProvider, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _logger = logger;

            /**
             * Always turn off AutoDetectChangesEnabled to improve performance
             * Must do DetectChanges() when commit
             */
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        /*****************************************************************
        Since using approach of:
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        the following method is not requied:

        public void EnableQueryTracking(bool enabled)
        {
            if (enabled)
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            else
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        *****************************************************************/

        public async Task<bool> CommitAsync()
        {
            bool result = false;
            try
            {
                _context.ChangeTracker.DetectChanges();
                EnsureSingleTenant();
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
                    case EntityState.Modified: // todo: or apply "entry.State == EntityState.Unchanged;" instead?
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
                /**
                 * Since _context.ChangeTracker.AutoDetectChangesEnabled is set to false by default,
                 * _context.ChangeTracker.DetectChanges() must be called before do _context.SaveChanges()
                 */
                _context.ChangeTracker.DetectChanges();

                if(!string.IsNullOrWhiteSpace(_tenantProvider?.GetTenantId()))
                {
                    MakeupMissingTenantIds();
                    EnsureSingleTenant();
                }

                _context.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return result;
        }

        private void MakeupMissingTenantIds()
        {
            var entities = from e in _context.ChangeTracker.Entries()
                           where e.Entity is TenantEntity && ((TenantEntity)e.Entity).TenantId == null
                           select (TenantEntity)e.Entity;

            foreach (var entity in entities)
            {
                if(string.IsNullOrWhiteSpace(entity.TenantId)) // if entity doesn't set tenant id
                    entity.TenantId = _tenantProvider.GetTenantId();
            }
        }

        /**
         * Make sure all the changes apply to the same tenant
         * 
         * from:
         *     Defensive database context for multi-tenant ASP.NET Core applications
         *     https://gunnarpeipman.com/aspnet-core-defensive-database-context/
         */
        private void EnsureSingleTenant()
        {
            if (_tenantProvider.GetTenantId() is null)
                return;

            var ids = (
                from e in _context.ChangeTracker.Entries()
                where e.Entity is TenantEntity
                select ((TenantEntity)e.Entity).TenantId
            ).Distinct().ToList();

            // TenantEntity is not used as the base class for other entities,
            // i.e. this is a "traditional" design without multi-tenant design
            if (ids.Count == 0)
                return;

            if (ids.Count > 1)
                throw new Exception("Updating multiple tenant"); // TODO: need to log tenant IDs

            if (ids.First() == null)
                throw new Exception("Invalid tenant ID: null");

            if (ids.First() != _tenantProvider.GetTenantId())
                throw new Exception($"Invalid tenant ID: {ids[0]}");
        }
    }
}
