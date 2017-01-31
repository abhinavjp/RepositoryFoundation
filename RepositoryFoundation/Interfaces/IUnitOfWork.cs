using RepositoryFoundation.Interfaces;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace RepositoryFoundation.Repository.Interface
{
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : IDbContext
    {
        IGenericRepository<TContext, TEntity, TIdType> GetRepository<TEntity, TIdType>(Func<TEntity, TIdType> idGetter) where TEntity : class;
        int CommitMultiple(params IUnitOfWork[] unitOfWorks);
        Task<int> CommitMultipleAsync(params IUnitOfWork[] unitOfWorks);
    }

    public interface IUnitOfWork: IDisposable
    {
        int Commit();
        Task<int> CommitAsync();
    }
}
