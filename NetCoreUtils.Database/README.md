# NetCoreUtils.Database

## About

## Release Notes

### v0.2.2.6

- bug fix: RepositoryBase.DbSet property mistakenly return itself
- update RepositoryBase.Delete(Expression<Func<TEntity, bool>> where) to use dbSet.RemoveRange()

### v0.2.1.5

- (not tested) Add RejectAllChanges() in IUnitOfWork

### v0.2.0.4

- Provide category name for injected logs of UnitOfWork and RepositoryBase
- Add a DbContext property for RepositoryBase

### v0.1.1.3

- Update dependent NetCoreUtils package