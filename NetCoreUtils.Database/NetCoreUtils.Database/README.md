# NetCoreUtils.Database

<!--TOC-->
  - [Overview](#overview)
  - [TODOs](#todos)
  - [Design Notes](#design-notes)
    - [Implementation of multi-tenancy](#implementation-of-multi-tenancy)
    - [Reasons of wrapping extra repositories for dbSets](#reasons-of-wrapping-extra-repositories-for-dbsets)
    - [Reasons of wrapping an extra IUnitOfWork for DbContext](#reasons-of-wrapping-an-extra-iunitofwork-for-dbcontext)
    - [Reasons of returning IQueryable<> in `Query()` and `QueryAll()](#reasons-of-returning-iqueryable-in-query-and-queryall)
  - [Release Notes](#release-notes)
  - [Usage](#usage)
<!--/TOC-->

## Overview

- Every entity's repository is committable.
- All repositories and UnitOfWork are injected as Scoped (using .net core build-in DI API "AddScoped").
- Decide to give up adding support for MongoDB as the MongoDB API is too
  different from that of Entity Framework's. Therefore library
  NetCoreUtils.Database is only used for relational database.

## TODOs

- Test: are DbContext of IRepositoryReadonly and IRepository the same instance?
- Test: TenantUtil.EnableMultiTenant
- Perform unit test on PostgreSQL
- Review multi-tenant implementation (use global filter)
- Move NetCoreUtils.Database to separate git repo, then use github action to release package
- Update unit test with performance benchmark
- Default to enable transaction in UnitOfWork  
  find more: https://learn.microsoft.com/en-us/ef/core/saving/

## Design Notes

### Implementation of multi-tenancy

- If an entity should have tenant information, it should inherit from `TenantEntity`
- (not tested) Call `TenantUtil.EnableMultiTenant(..)` in
  `DbContext.OnModelCreating(..)` to register global query filter

- In `UnitOfWork.CommitAsync()`, a `ConfirmSingleTenant()` will be called to make sure all the
  updated Entities use the same tenant ID if the tenant ID is available.

### Reasons of wrapping extra repositories for dbSets

- Able to view clearer repository/dbset dependency relationship from a service's
  constructor

- Able to declare a readonly repository by using IRepositoryReadonly

### Reasons of wrapping an extra IUnitOfWork for DbContext

- Improve performance by disabling `ChangeTracker.AutoDetectChangesEnabled` by default

- Help for debugging. The SaveChanges() is wrapped in a try..catch block, if something's wrong,
  the error message will be output to the injected logger. And it also helps to intercept the error
  by put a breakpoint in the catch section.

- The RejectAllChanges() method

### Reasons of returning IQueryable<> in `Query()` and `QueryAll()

- Allow the invocation client to use ".Include()" method to load an entity's navigation
  properties

- Allow to use the dbSet in a linq statement without loading all relevant data into the
  memory, which is the behavior if one uses IEnumerable instead of IQueryable

## Release Notes

See [release notes](./release-notes.md)

## Usage

- If one job involes multiple repositories, the last repository shall be
  responsible for committing the unit of work for all. 

- Register DI by:
  ```
  services.AddDbContext<ApplicationDbContext>();
  services.AddRepositories<ApplicationDbContext>();
  ```
  Then inject `IRepository<Entity>` or `IRepositoryReadonly<Entity>` in program.
