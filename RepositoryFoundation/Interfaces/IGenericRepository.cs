using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryFoundation.Repository.Interface
{
    public interface IGenericRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
    {
        IQueryable<TEntity> All { get; }
        IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> FindWhere(Expression<Func<TEntity, bool>> conditionParam);
        TEntity Find(int id);
        void Insert(TEntity TEntry);
        void Update(TEntity TEntry);
        void Delete(int id);
    }
}
