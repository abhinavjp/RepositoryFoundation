using System;
using System.Data.Entity;

namespace RepositoryFoundation.Repository.Interface
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        IGenericRepository<TContext, TEntity, TIdType> GetRepository<TEntity, TIdType>(Func<TEntity, TIdType> idGetter) where TEntity : class;
        void Commit();
    }
}
