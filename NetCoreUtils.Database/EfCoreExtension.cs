using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.Database
{
    /// <summary>
    /// from: http://blog.cincura.net/233451-using-entity-frameworks-find-method-with-predicate/
    /// </summary>
    static public class EfCoreExtension
    {
        /// <summary>
        /// from: http://stackoverflow.com/questions/33819159/is-there-a-dbsettentity-local-equivalent-in-entity-framework-7
        /// </summary>
        private static ObservableCollection<TEntity> GetLocal<TEntity>(this DbSet<TEntity> set) where TEntity : class
        {
            var context = set.GetService<DbContext>();
            var data = context.ChangeTracker.Entries<TEntity>().Select(e => e.Entity);
            var collection = new ObservableCollection<TEntity>(data);

            collection.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                    context.AddRange(e.NewItems.Cast<TEntity>());

                if (e.OldItems != null)
                    context.RemoveRange(e.OldItems.Cast<TEntity>());
            };

            return collection;
        }

        // There's no need to have a FindLocalFirstAsync() as we can do FindLocalFirst(..).ToListAsync()
        public static IQueryable<T> FindLocalFirst<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> predicate) where T : class
        {
            // TODO - now dbSet seems to have a 'Local' member, test it: var local2 = dbSet.Local.Where(predicate.Compile());
            var local = dbSet.GetLocal().Where(predicate.Compile()); // query 'Local' to see if data has been loaded
            if (local.Any())
                return local.AsQueryable();
            else
                return dbSet.Where(predicate); // load data from the database
        }
    }

}
