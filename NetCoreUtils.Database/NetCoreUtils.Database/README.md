# NetCoreUtils.Database

## Overview

- Every entity's repository is committable.
- All repositories and UnitOfWork are injected as Scoped (using .net core build-in DI API "AddScoped").

## TODOs

- Merge EfRepository into Repository, and remove EfRepository
  
  Decide to give up adding support for MongoDB as the MongoDB API is too different from that of Entity Framework's.

  Therefore library NetCoreUtils.Database is only used for relational database.

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

## Release Notes

See [release notes](./release-notes.md)

## Usage

- If one job involes multiple repositories, the last repository shall be
  responsible for committing the unit of work for all. 
- Call `services.AddRepositories<ApplicationDbContext>()` to register
  dependencies. This method is implemented in `Extensions.cs`.
- A `services.AddDbContext<ApplicationDbContext>()` must be called
