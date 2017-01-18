using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryFoundation.Repository.Interface
{
    public interface IGenericRepository<TEntity, TContext, TIdType> where TEntity : class where TContext : DbContext
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> FindWhere(Expression<Func<TEntity, bool>> conditionParam);
        TEntity FindFirst(Expression<Func<TEntity, bool>> conditionParam);
        TEntity Find(int id);
        void InsertOrUpdate(TEntity TEntry);
        void Delete(int id);
    }
}
