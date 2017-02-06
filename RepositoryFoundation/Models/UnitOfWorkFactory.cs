using RepositoryFoundation.Interfaces;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RepositoryFoundation.Repository.Infrastructure.StructureMapConfigurator;

namespace RepositoryFoundation.Models
{
    public class UnitOfWorkFactory
    {
        private readonly Dictionary<DbContext, IUnitOfWork<DbContext>> _unitOfWorkDictionary;
        public UnitOfWorkFactory()
        {
            _unitOfWorkDictionary = new Dictionary<DbContext, IUnitOfWork<DbContext>>();
        }
        public IUnitOfWork<TContext> GetUnitOfWork<TContext>(TContext context) where TContext : DbContext
        {
            if (!_unitOfWorkDictionary.ContainsKey(context))
            {
                var args = new ExplicitArguments();
                args.Set(context);
                _unitOfWorkDictionary[context] = GetInstance<IUnitOfWork<TContext>>(args) as IUnitOfWork<DbContext>;
            }
            return _unitOfWorkDictionary[context] as IUnitOfWork<TContext>;
        }
        public int Commit<TContext>(TContext context) where TContext : DbContext
        {
            if (!_unitOfWorkDictionary.ContainsKey(context))
            {
                throw new KeyNotFoundException("No Unit Of Work found corresponding to the context");
            }
            return _unitOfWorkDictionary[context].Commit();
        }
        public async Task<int> CommitAsync<TContext>(TContext context) where TContext : DbContext
        {
            if (!_unitOfWorkDictionary.ContainsKey(context))
            {
                throw new KeyNotFoundException("No Unit Of Work found corresponding to the context");
            }
            return await _unitOfWorkDictionary[context].CommitAsync();
        }
        public int CommitAll()
        {
            int count = 0;
            foreach(var unitOfWorkItem in _unitOfWorkDictionary)
            {
                count += unitOfWorkItem.Value.Commit();
            }
            return count;
        }
        public async Task<int> CommitAllAsync()
        {
            int count = 0;
            foreach (var unitOfWorkItem in _unitOfWorkDictionary)
            {
                count += await unitOfWorkItem.Value.CommitAsync();
            }
            return count;
        }
    }
}
