using System.Data.Entity;

namespace RepositoryFoundation.Repository.Interface
{
    public interface IUnitOfWork<TContext, TEntity, TIdType> where TContext : DbContext where TEntity : class
    {
        IGenericRepository<TEntity, TContext> GetRepository();
        void Commit();
    }
}
