# Release Notes

## v2.1.0

Major Enhancement:

- Add multi-tenant support (mainly for UnitOfWork)

Other Enhancement:

- Add package dependency `Npgsql.EntityFrameworkCore.PostgreSQL`
- Add package dependency `Microsoft.EntityFrameworkCore.SqlServer`
- Add package dependency `Microsoft.EntityFrameworkCore.Sqlite`

## v2.0.0

Major Enhancement:

- Remove TDbContext generic type parameter, so RepositoryReader<TEntity>,
  RepositoryWrite<TEntity> and Repository<TEntity> are not needed to be defined
  externally
- Merge IEfRepositoryRead<> into IRepositoryRead<>, and delete IEfRepositoryRead<> and IEfRepository<>

Other Enhancement:

- Upgrade to .net5
- Upgrade EF Core dependency from v2.1 to v5.0.7
- Upgrade NetCoreUtils from 0.3.0.4 to 1.2.0
- Add package dependency `Microsoft.EntityFrameworkCore.InMemory`
- Add package dependency `Microsoft.EntityFrameworkCore.Relational` to enable
  `DbContext.Database.MigrateAsync()` method

## v1.1.0

- Stop exposing DbContext from IRepository<>
- Remove useless unitOfWork and dbSet members from Repository<,> implementation
- Add "overview" and "design" sections for this readme document.

## v1.0.2

- Remove unnecessary dependency to CoreCmd

## v1.0.1

- Add dependency injection extension method
- Change version schema to three digits

## v0.4.0.9

Enhancement:

- Upgrade to .net core 2.2
- Add interfaces with the same generic template parameter number for easy DI registration like:
  services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>))
  services.AddScoped(typeof(IRepositoryWrite<,>), typeof(RepositoryWrite<,>));
  services.AddScoped(typeof(IRepositoryRead<,>), typeof(RepositoryRead<,>));

Incompatible changes:

- Rename RepositoryBase to Repository

## v0.3.0.8

- Segregate read and write operations into separate interfaces and class implementations
- Declare all methods in the implementation classes as "virtual"
- Remove "abstract" from RepositoryBase<> definition

Incompatible changes:

- Remove IDisposable from Committable since EF DbContext can manage itself and an DI framework
  can help to dispose an injected DbContext as well
- Move EnableQueryTracking() to IUnitOfWork
- "Delete" methods are renamed to "Remove" to keep consistant with EF's method naming

## v0.2.3.7

- (not tested) Enhancement: add NoTracking methods

## v0.2.2.6

- Bug fix: RepositoryBase.DbSet property mistakenly return itself
- Update: RepositoryBase.Delete(Expression<Func<TEntity, bool>> where) to use dbSet.RemoveRange()

## v0.2.1.5

- (not tested) Enhancement: Add RejectAllChanges() in IUnitOfWork

## v0.2.0.4

- Enhancement: Provide category name for injected logs of UnitOfWork and RepositoryBase
- Enhancement: Add a DbContext property for RepositoryBase

## v0.1.1.3

- Update: dependent NetCoreUtils package
