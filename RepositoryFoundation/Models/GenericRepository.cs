using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using RepositoryFoundation.Repository.Interface;
using RepositoryFoundation.Interfaces;

namespace RepositoryFoundation.Repository.Models
{
    public class GenericRepository<TContext, TEntity, TIdType> : IGenericRepository<TContext, TEntity, TIdType> where TEntity : class where TContext : IDbContext
    {
        internal TContext context;
        internal IDbSet<TEntity> dbSet;
        internal Func<TEntity, TIdType> idGetter;

        public GenericRepository(TContext context, Func<TEntity, TIdType> idGetter)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
            this.idGetter = idGetter;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return dbSet;
        }

        public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual IQueryable<TEntity> FindWhere(Expression<Func<TEntity, bool>> conditionParam)
        {
            IQueryable<TEntity> query = dbSet;
            query = query.Where(conditionParam);
            return query;
        }

        public virtual TEntity FindFirst(Expression<Func<TEntity, bool>> conditionParam)
        {
            return dbSet.FirstOrDefault(conditionParam);
        }

        public virtual TEntity FindFirst()
        {
            return dbSet.FirstOrDefault();
        }

        public virtual bool HasData(Expression<Func<TEntity, bool>> conditionParam)
        {
            return dbSet.Any(conditionParam);
        }

        public virtual bool HasData()
        {
            return dbSet.Any();
        }

        public virtual TEntity Find(int id)
        {
            return dbSet.Find(id);
        }

        public virtual void InsertOrUpdate(TEntity TEntry)
        {
            if (idGetter(TEntry).Equals(default(TIdType)))
            {
                dbSet.Add(TEntry);
            }
            else
            {
                if (context.Entry(TEntry).State == EntityState.Detached)
                {
                    dbSet.Attach(TEntry);
                }
                context.Entry(TEntry).State = EntityState.Modified;
            }
        }

        public virtual void InsertOrUpdate(params TEntity[] TEntries)
        {
            foreach (var TEntry in TEntries)
            {
                InsertOrUpdate(TEntry);
            }
        }

        public virtual void Delete(int id)
        {
            var item = dbSet.Find(id);
            dbSet.Remove(item);
        }
    }
}
