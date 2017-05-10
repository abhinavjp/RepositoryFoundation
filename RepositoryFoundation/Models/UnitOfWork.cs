using RepositoryFoundation.Interfaces;
using StructureMap.Pipeline;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using static RepositoryFoundation.Repository.Infrastructure.StructureMapConfigurator;

namespace RepositoryFoundation.Repository.Models
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context;

        public UnitOfWork(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("Context was not supplied");
            }
            _context = context;
        }
        public IGenericRepository<TContext, TEntity, TIdType> GetRepository<TEntity, TIdType>(Func<TEntity, TIdType> idGetter) where TEntity : class
        {
            var args = new ExplicitArguments();
            args.Set(_context);
            args.Set(idGetter);
            return GetInstance<IGenericRepository<TContext, TEntity, TIdType>>(args);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int CommitMultiple(params IUnitOfWork[] unitOfWorks)
        {
            int count = 0;
            foreach (var unitOfWork in unitOfWorks)
            {
                count += unitOfWork.Commit();
            }
            return count;
        }

        public async Task<int> CommitMultipleAsync(params IUnitOfWork[] unitOfWorks)
        {
            int count = 0;
            foreach (var unitOfWork in unitOfWorks)
            {
                count += await unitOfWork.CommitAsync();
            }
            return count;
        }

        public void SetLogger(Action<string> logger)
        {
            _context.Database.Log = logger;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
