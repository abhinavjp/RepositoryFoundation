using RepositoryFoundation.Repository.Interface;
using StructureMap.Pipeline;
using System;
using System.Data.Entity;
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
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
    }
}
