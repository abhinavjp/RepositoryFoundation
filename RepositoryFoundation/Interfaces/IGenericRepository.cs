using RepositoryFoundation.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryFoundation.Repository.Interface
{
    public interface IGenericRepository<TContext, TEntity, TIdType> where TEntity : class where TContext : IDbContext
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> FindWhere(Expression<Func<TEntity, bool>> conditionParam);
        TEntity FindFirst(Expression<Func<TEntity, bool>> conditionParam);
        TEntity FindFirst();
        TEntity Find(int id);
        bool HasData(Expression<Func<TEntity, bool>> conditionParam);
        bool HasData();
        void InsertOrUpdate(TEntity TEntry);
        void Delete(int id);
    }
}
