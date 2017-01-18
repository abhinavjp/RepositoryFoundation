using RepositoryFoundation.Repository.Interface;
using StructureMap.Pipeline;
using System;
using System.Data.Entity;
using static RepositoryFoundation.Repository.Infrastructure.StructureMapConfigurator;

namespace RepositoryFoundation.Repository.Models
{
    public class UnitOfWork<TContext, TEntity, TIdType> : IUnitOfWork<TContext, TEntity, TIdType>, IDisposable where TContext: DbContext where TEntity : class
    {
        private readonly TContext _context;

        public UnitOfWork(TContext context, Func<TEntity, TIdType> idGetter)
        {
            if (context == null)
            {
                throw new ArgumentNullException("Context was not supplied");
            }
            _context = context;
        }
        public IGenericRepository<TEntity, TContext, TIdType> GetRepository()
        {
            var args = new ExplicitArguments();
            args.Set(_context);
            return GetInstance<IGenericRepository<TEntity, TContext, TIdType>>(args);
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
