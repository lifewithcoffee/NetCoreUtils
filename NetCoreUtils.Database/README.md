# NetCoreUtils.Database

## About

## Release Notes

### (working)

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

### v0.3.0.8-working

- Segregate read and write operations into separate interfaces and class implementations
- Declare all methods in the implementation classes as "virtual"
- Remove "abstract" from RepositoryBase<> definition

*Incompatible changes:*

- Remove IDisposable from Committable since EF DbContext can manage itself and an DI framework can help to dispose an injected DbContext as well
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

- Add local single template type parameter `Repository<TEntity>` implementatiion (without TDbContext):

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

- Add DI registration:

``` c#
  services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
  services.AddScoped(typeof(IRepositoryRead<,>), typeof(RepositoryRead<,>));
  services.AddScoped(typeof(IRepositoryWrite<,>), typeof(RepositoryWrite<,>));

  services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
  services.AddScoped(typeof(IRepositoryRead<>), typeof(RepositoryReader<>));
  services.AddScoped(typeof(IRepositoryWrite<>), typeof(RepositoryWriter<>));
```

