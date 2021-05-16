using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
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

    static public class ServiceExtension
    {
        /** To register a single generic type interface of IRepository<TEntity>, like:
         * 
         *      services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
         *      services.AddScoped(typeof(IRepositoryRead<>), typeof(RepositoryReader<>));
         *      services.AddScoped(typeof(IRepositoryWrite<>), typeof(RepositoryWriter<>));
         *  
         * Need to delcare the following classes in the outer application:
         * 
         *      public class RepositoryReader<TEntity>
         *          : RepositoryRead<TEntity, ApplicationDbContext>
         *          where TEntity : class
         *      {
         *          public RepositoryReader(IUnitOfWork<ApplicationDbContext> unitOfWork)
         *              : base(unitOfWork)
         *          { }
         *      }
         *    
         *      public class RepositoryWriter<TEntity>
         *          : RepositoryWrite<TEntity, ApplicationDbContext>
         *          where TEntity : class
         *      {
         *          public RepositoryWriter(IUnitOfWork<ApplicationDbContext> unitOfWork)
         *              : base(unitOfWork)
         *          { }
         *      }
         *    
         *      public class Repository<TEntity>
         *          : Repository<TEntity, ApplicationDbContext>
         *          where TEntity : class
         *      {
         *          public Repository(
         *              IUnitOfWork<ApplicationDbContext> unitOfWork,
         *              IRepositoryRead<TEntity, ApplicationDbContext> repoReader,
         *              IRepositoryWrite<TEntity, ApplicationDbContext> repoWriter
         *          ) : base(unitOfWork, repoReader, repoWriter)
         *          { }
         *      }
         */
        static public void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IRepositoryRead<,>), typeof(RepositoryRead<,>));
            services.AddScoped(typeof(IRepositoryWrite<,>), typeof(RepositoryWrite<,>));

            services.AddScoped(typeof(IEfRepository<,>), typeof(EfRepository<,>));
            services.AddScoped(typeof(IEfRepositoryRead<,>), typeof(EfRepositoryRead<,>));
        }
    }
}
