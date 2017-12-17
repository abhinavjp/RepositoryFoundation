using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using RepositoryFoundation.Interfaces;

namespace RepositoryFoundation.Models
{
    public class GenericRepository<TContext, TEntity, TIdType> : IGenericRepository<TContext, TEntity, TIdType> where TEntity : class where TContext : DbContext where TIdType : struct
    {
        internal TContext Context;
        internal DbSet<TEntity> DbSet;
        internal Func<TEntity, TIdType> IdGetter;

        public GenericRepository(TContext context, Func<TEntity, TIdType> idGetter)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context), "Context was not supplied");
            DbSet = context.Set<TEntity>();
            IdGetter = idGetter ?? throw new ArgumentNullException(nameof(idGetter), "The func delegate through which Id can be fetched was not supplied");
        }

        public virtual IQueryable<TEntity> All() => DbSet;

        public virtual IQueryable<TEntity> All(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(DbSet, (current, includeProperty) => current.Include(includeProperty));
        }

        public virtual IQueryable<TEntity> All(params string[] includePropertiesPath)
        {
            IQueryable<TEntity> query = DbSet;
            foreach (var includeProperty in includePropertiesPath)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> conditionParam)
        {
            IQueryable<TEntity> query = DbSet;
            query = query.Where(conditionParam);
            return query;
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> conditionParam)
        {
            return DbSet.FirstOrDefault(conditionParam);
        }

        public virtual TEntity FirstOrDefault()
        {
            return DbSet.FirstOrDefault();
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> conditionParam)
        {
            return DbSet.Any(conditionParam);
        }

        public virtual bool Any()
        {
            return DbSet.Any();
        }

        public virtual TEntity Find(int id)
        {
            return DbSet.Find(id);
        }

        public virtual void InsertOrUpdate(TEntity entry)
        {
            if (IdGetter != null && IdGetter(entry).Equals(default(TIdType)))
            {
                DbSet.Add(entry);
            }
            else
            {
                if (Context.Entry(entry).State == EntityState.Detached)
                {
                    DbSet.Attach(entry);
                }
                Context.Entry(entry).State = EntityState.Modified;
            }
        }

        public void InsertOrUpdateMultiple(params TEntity[] entries)
        {
            foreach (var entry in entries)
            {
                InsertOrUpdate(entry);
            }
        }

        public void InsertOrUpdateMultiple(IList<TEntity> entries)
        {
            InsertOrUpdateMultiple(entries.ToArray());
        }

        public virtual void Insert(TEntity entry)
        {
            DbSet.Add(entry);
        }

        public virtual void InsertMultiple(params TEntity[] entries)
        {
            DbSet.AddRange(entries.Where(t => IdGetter != null && IdGetter(t).Equals(default(TIdType))));
        }

        public void InsertMultiple(IList<TEntity> entries)
        {
            DbSet.AddRange(entries.Where(t => IdGetter != null && IdGetter(t).Equals(default(TIdType))));
        }

        public virtual void Update(TEntity entry)
        {
            if (Context.Entry(entry).State == EntityState.Detached)
            {
                DbSet.Attach(entry);
            }
            Context.Entry(entry).State = EntityState.Modified;
        }

        public virtual void UpdateMultiple(params TEntity[] entries)
        {
            foreach (var entry in entries)
            {
                Update(entry);
            }
        }

        public void UpdateMultiple(IList<TEntity> entries)
        {
            UpdateMultiple(entries.ToArray());
        }

        public virtual void Delete(TIdType id)
        {
            var item = DbSet.Find(id);
            if (item != null)
                DbSet.Remove(item);
        }

        public virtual void DeleteMultiple(params TIdType[] id)
        {
            var item = DbSet.Where(tw => IdGetter != null && id.Contains(IdGetter(tw)));
            DbSet.RemoveRange(item);
        }

        public void SetCommandTimeout(int timeOut)
        {
            Context.Database.CommandTimeout = timeOut;
        }

        public void SetLogger(Action<string> logger)
        {
            Context.Database.Log = logger;
        }
    }
}
