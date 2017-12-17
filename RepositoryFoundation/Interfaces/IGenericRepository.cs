using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryFoundation.Interfaces
{
    public interface IGenericRepository<TContext, TEntity, in TIdType> where TEntity : class where TIdType: struct
    {
        IQueryable<TEntity> All();
        IQueryable<TEntity> All(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> All(params string[] includePropertiesPath);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> conditionParam);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> conditionParam);
        TEntity FirstOrDefault();
        TEntity Find(int id);
        bool Any(Expression<Func<TEntity, bool>> conditionParam);
        bool Any();
        void InsertOrUpdate(TEntity entry);
        void InsertOrUpdateMultiple(params TEntity[] entries);
        void InsertOrUpdateMultiple(IList<TEntity> entries);
        void Insert(TEntity entry);
        void InsertMultiple(params TEntity[] entries);
        void InsertMultiple(IList<TEntity> entries);
        void Update(TEntity entry);
        void UpdateMultiple(params TEntity[] entries);
        void UpdateMultiple(IList<TEntity> entries);
        void Delete(TIdType id);
        void DeleteMultiple(params TIdType[] id);
        void SetCommandTimeout(int timeOut);
        void SetLogger(Action<string> logger);
    }
}
