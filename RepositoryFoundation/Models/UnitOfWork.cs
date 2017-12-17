using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RepositoryFoundation.Infrastructure;
using RepositoryFoundation.Interfaces;

namespace RepositoryFoundation.Models
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        private readonly TContext _context;

        public UnitOfWork()
        {
            _context = new TContext();
        }

        public static IUnitOfWork<TContext> Instance => UnitOfWorkConfigurator.GetInstance<IUnitOfWork<TContext>>();

        public IGenericRepository<TContext, TEntity, TIdType> GetRepository<TEntity, TIdType>(Func<TEntity, TIdType> idGetter) where TEntity : class where TIdType : struct
        {
            return UnitOfWorkConfigurator.GetInstance<IGenericRepository<TContext, TEntity, TIdType>>(_context, idGetter);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual void SetLogger(Action<string> logger)
        {
            _context.Database.Log = logger;
        }

        public void SetCommandTimeout(int timeOut)
        {
            _context.Database.CommandTimeout = timeOut;
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}