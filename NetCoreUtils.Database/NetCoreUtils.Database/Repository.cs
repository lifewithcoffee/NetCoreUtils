
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetCoreUtils.Database;
public class Repository<TEntity> : RepositoryReadable<TEntity>, IRepository<TEntity> where TEntity : class
{
    public Repository(IUnitOfWork unitOfWork):base(unitOfWork)
    { }

    /// <summary> Reason of not implementing AddAsync:
    /// According to: http://stackoverflow.com/questions/42034282/are-there-dbset-updateasync-and-removeasync-in-net-core
    /// DbSet.AddAsync() should not be used.
    /// 
    /// Quote:
    ///
    /// AddAsync however, only begins tracking an entity but won't actually send any changes 
    /// to the database until you call SaveChanges or SaveChangesAsync. You shouldn't really 
    /// be using this method unless you know what you're doing. The reason the async version 
    /// of this method exists is explained in the docs:
    /// 
    /// This method is async only to allow special value generators, such as the one used by
    /// 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
    /// to access the database asynchronously. For all other cases the non async method should
    /// be used.
    /// 
    /// The same reason is also applied to AddRagne, Remove and Update methods.
    /// </summary>
    public virtual TEntity Add(TEntity entity)
    {
        _dbSet.Add(entity);
        return entity;
    }

    /// <summary> Do not use AddRagneAsync() if not necessary
    /// According to: https://stackoverflow.com/questions/56241351/what-is-the-difference-between-addrange-and-addrangeasync-in-ef-core
    /// AddRangeAsync() may be slower and should only used when value
    /// generators need access to the database before they generate a
    /// value.
    /// 
    /// The reason looks the same with that of AddAsync()
    /// 
    /// </summary>
    public virtual void AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public virtual void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void Remove(Expression<Func<TEntity, bool>> where)
    {
        IEnumerable<TEntity> objects = _dbSet.Where<TEntity>(where).AsEnumerable();
        _dbSet.RemoveRange(objects);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public virtual void Update(TEntity entity)
    {
       /**
        * For reference, the old impl in EF6:
        *
        * dbSet.Attach(entity);
        * _unitOfWork.Context.Entry(entity).State = EntityState.Modified; 
        */

        _dbSet.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public virtual bool Commit()
    {
        return this._unitOfWork.Commit();
    }

    public virtual async Task<bool> CommitAsync()
    {
        return await this._unitOfWork.CommitAsync();
    }

    public void RemoveAll()
    {
        _dbSet.RemoveRange(_dbSet);
    }
}