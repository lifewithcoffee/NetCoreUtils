namespace NetCoreUtils.Database
{
    public interface IRepository<TEntity> :
        IRepositoryReadable<TEntity>,
        IRepositoryWritable<TEntity> where TEntity : class
    {
    }

}