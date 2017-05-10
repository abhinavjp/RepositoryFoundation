using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace RepositoryFoundation.Interfaces
{
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        IGenericRepository<TContext, TEntity, TIdType> GetRepository<TEntity, TIdType>(Func<TEntity, TIdType> idGetter) where TEntity : class;
        int CommitMultiple(params IUnitOfWork[] unitOfWorks);
        Task<int> CommitMultipleAsync(params IUnitOfWork[] unitOfWorks);
    }

    public interface IUnitOfWork: IDisposable
    {
        int Commit();
        Task<int> CommitAsync();
        void SetLogger(Action<string> logger);
    }
}
