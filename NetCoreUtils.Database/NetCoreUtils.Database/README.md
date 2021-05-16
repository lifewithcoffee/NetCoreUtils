# NetCoreUtils.Database

## Overview

- Every entity's repository is committable.
- All repositories and UnitOfWork are injected as Scoped (using .net core build-in DI API "AddScoped").

## Design Thoughts

### Reason of wrapping extra repositories for dbSets

The main reason is for readibility.

Just like using constructor dependency injection, by wrapping individual entities in to repositories,
and injecting them from a class's constructor, it will be very clear helpful to understand which entities
the business logic will perform data access to.

You can even further to dependent on only IRepositoryRead<> or IRepositoryWrite<> to express clearer
about which operation (read or write) the business logic will depend on.

### Reason of wrapping an extra IUnitOfWork for DbContext

Two reasons:

- Help for debugging. The SaveChanges() is wrapped in a try..catch block, if something's wrong,
  the error message will be output to the injected logger. And it also helps to intercept the error
  by put a breakpoint in the catch section.

- The RejectAllChanges() method

### Reason of returning IQueryable<> in `Query()` and `QueryAll()

Two reasons:

- Allow the invocation client to use ".Include()" method to load an entity's navigation
  properties
- Allow to use the dbSet in a linq statement without loading all relevant data into the
  memory, which is the behavior if one uses IEnumerable instead of IQueryable

## Usage Guides

- If one job involes multiple repositories, the last repository shall be responsible for
  committing the unit of work for all. 

## Release Notes

### v1.2 (working)

- Upgrade to .net core 3.0

## v1.1

- Stop exposing DbContext from IRepository<>
- Remove useless unitOfWork and dbSet members from Repository<,> implementation
- Add "overview" and "design" sections for this readme document.

### v1.0.2

- Remove unnecessary dependency to CoreCmd

### v1.0.1

- Add dependency injection extension method
- Change version schema to three digits

### v0.4.0.9

*Enhancement:*

- Upgrade to .net core 2.2
- Add interfaces with the same generic template parameter number for easy DI registration like:
  services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>))
  services.AddScoped(typeof(IRepositoryWrite<,>), typeof(RepositoryWrite<,>));
  services.AddScoped(typeof(IRepositoryRead<,>), typeof(RepositoryRead<,>));

*Incompatible changes:*

- Rename RepositoryBase to Repository

### v0.3.0.8

- Segregate read and write operations into separate interfaces and class implementations
- Declare all methods in the implementation classes as "virtual"
- Remove "abstract" from RepositoryBase<> definition

*Incompatible changes:*

- Remove IDisposable from Committable since EF DbContext can manage itself and an DI framework
  can help to dispose an injected DbContext as well
- Move EnableQueryTracking() to IUnitOfWork
- "Delete" methods are renamed to "Remove" to keep consistant with EF's method naming

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

## Usage

### Register basic dependencies

(Implemented in Extensions.cs)

``` c#
  services.AddRepositories();
```

### Add local `Repository<TEntity>` implementatiion

``` c#
  public class RepositoryReader<TEntity>
      : RepositoryRead<TEntity, ApplicationDbContext>
      where TEntity : class
  {
      public RepositoryReader(IUnitOfWork<ApplicationDbContext> unitOfWork)
          : base(unitOfWork)
      { }
  }

  public class RepositoryWriter<TEntity>
      : RepositoryWrite<TEntity, ApplicationDbContext>
      where TEntity : class
  {
      public RepositoryWriter(IUnitOfWork<ApplicationDbContext> unitOfWork)
          : base(unitOfWork)
      { }
  }

  public class RepositoryBase<TEntity>
      : RepositoryBase<TEntity, ApplicationDbContext>
      where TEntity : class
  {
      public RepositoryBase(
          IUnitOfWork<ApplicationDbContext> unitOfWork,
          IRepositoryRead<TEntity, ApplicationDbContext> repoReader,
          IRepositoryWrite<TEntity, ApplicationDbContext> repoWriter
      ) : base(unitOfWork, repoReader, repoWriter)
      { }
  }
```

then register the dependency:

``` c#
  services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
  services.AddScoped(typeof(IRepositoryRead<>), typeof(RepositoryReader<>));
  services.AddScoped(typeof(IRepositoryWrite<>), typeof(RepositoryWriter<>));
```

