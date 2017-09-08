using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryFoundation.Interfaces
{
    public interface IGenericRepository<TContext, TEntity, TIdType> where TEntity : class where TContext : DbContext where TIdType: struct
    {
        IQueryable<TEntity> All { get; }
        IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> conditionParam);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> conditionParam);
        TEntity FirstOrDefault();
        TEntity Find(int id);
        bool Any(Expression<Func<TEntity, bool>> conditionParam);
        bool Any();
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
