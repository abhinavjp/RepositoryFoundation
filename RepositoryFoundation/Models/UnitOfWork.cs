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
        private TContext _context;
        public static IUnitOfWork<TContext> NewInstance
        {
            get
            {
                var context = Activator.CreateInstance<TContext>();
                return new UnitOfWork<TContext>(context);
            }
        }
        private UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Context was not supplied");
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
            var count = 0;
            foreach (var unitOfWork in unitOfWorks)
            {
                count += unitOfWork.Commit();
            }
            return count;
        }

        public async Task<int> CommitMultipleAsync(params IUnitOfWork[] unitOfWorks)
        {
            var count = 0;
            foreach (var unitOfWork in unitOfWorks)
            {
                count += await unitOfWork.CommitAsync();
            }
            return count;
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
            if (_context != null)
                _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
