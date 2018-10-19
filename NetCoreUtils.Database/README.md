# NetCoreUtils.Database

## About

## Release Notes

### v0.2.3.7

- (not tested) Enhancement: add NoTracking methods

### v0.2.2.6

- Bug fix: RepositoryBase.DbSet property mistakenly return itself
- Update: RepositoryBase.Delete(Expression<Func<TEntity, bool>> where) to use dbSet.RemoveRange()

### v0.2.1.5

- (not tested) Enhancement: Add RejectAllChanges() in IUnitOfWork

### v0.2.0.4

- Enhancement: Provide category name for injected logs of UnitOfWork and RepositoryBase
- Enhancement: Add a DbContext property for RepositoryBase

### v0.1.1.3

- Update: dependent NetCoreUtils package