using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace RepositoryFoundation.Interfaces
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        IGenericRepository<TContext, TEntity, TIdType> GetRepository<TEntity, TIdType>(Func<TEntity, TIdType> idGetter) where TEntity : class where TIdType : struct;
        int Commit();
        Task<int> CommitAsync();
        void SetLogger(Action<string> logger);
        void SetCommandTimeout(int timeOut);
    }
}
