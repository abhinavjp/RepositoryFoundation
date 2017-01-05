using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using RepositoryFoundation.Repository.Interface;

namespace RepositoryFoundation.Repository.Models
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
    {
        internal TContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(TContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> All
        {
            get { return dbSet; }
        }

        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<TEntity> FindWhere(Expression<Func<TEntity, bool>> conditionParam)
        {
            IQueryable<TEntity> query = dbSet;
            query = query.Where(conditionParam);
            return query;
        }

        public TEntity Find(int id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity TEntry)
        {
            dbSet.Add(TEntry);
        }

        public virtual void Update(TEntity TEntry)
        {
            context.Entry(TEntry).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = dbSet.Find(id);
            dbSet.Remove(item);
        }
    }
}
