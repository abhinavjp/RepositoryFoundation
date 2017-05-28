using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryFoundation.Interfaces
{
    public interface IGenericRepository<TContext, TEntity, TIdType> where TEntity : class where TContext : DbContext
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> FindWhere(Expression<Func<TEntity, bool>> conditionParam);
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> conditionParam);
        TEntity GetFirstOrDefault();
        TEntity Find(int id);
        bool HasAny(Expression<Func<TEntity, bool>> conditionParam);
        bool HasAny();
        void InsertOrUpdate(TEntity TEntry);
        void InsertOrUpdateMultiple(params TEntity[] TEntries);
        void InsertOrUpdateMultiple(IList<TEntity> TEntries);
        void Insert(TEntity TEntry);
        void InsertMultiple(params TEntity[] TEntries);
        void InsertMultiple(IList<TEntity> TEntries);
        void Update(TEntity TEntry);
        void UpdateMultiple(params TEntity[] TEntries);
        void UpdateMultiple(IList<TEntity> TEntries);
        void Delete(TIdType id);
        void DeleteMultiple(params TIdType[] id);
        void SetCommandTimeout(int timeOut);
        void SetLogger(Action<string> logger);
    }
}
