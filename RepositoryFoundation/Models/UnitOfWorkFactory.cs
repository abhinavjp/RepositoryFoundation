using RepositoryFoundation.Interfaces;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using static RepositoryFoundation.Infrastructure.UnitOfWorkConfigurator;

namespace RepositoryFoundation.Models
{
    public class UnitOfWorkFactory : IDisposable
    {
        private readonly Dictionary<Type, IUnitOfWork<DbContext>> _unitOfWorkDictionary;
        public UnitOfWorkFactory()
        {
            _unitOfWorkDictionary = new Dictionary<Type, IUnitOfWork<DbContext>>();
        }
        public IUnitOfWork<TContext> GetUnitOfWork<TContext>(TContext context) where TContext : DbContext
        {
            if (!_unitOfWorkDictionary.ContainsKey(context.GetType()))
            {
                var args = new ExplicitArguments();
                args.Set(context);
                _unitOfWorkDictionary[context.GetType()] = GetInstance<IUnitOfWork<TContext>>(args) as IUnitOfWork<DbContext>;
            }
            return _unitOfWorkDictionary[context.GetType()] as IUnitOfWork<TContext>;
        }

        public int Commit<TContext>() where TContext : DbContext
        {
            if (!_unitOfWorkDictionary.ContainsKey(typeof(TContext)))
            {
                throw new KeyNotFoundException("No Unit Of Work found corresponding to the context");
            }
            return _unitOfWorkDictionary[typeof(TContext)].Commit();
        }

        public async Task<int> CommitAsync<TContext>() where TContext : DbContext
        {
            if (!_unitOfWorkDictionary.ContainsKey(typeof(TContext)))
            {
                throw new KeyNotFoundException("No Unit Of Work found corresponding to the context");
            }
            return await _unitOfWorkDictionary[typeof(TContext)].CommitAsync();
        }

        public int CommitMultiple(params Type[] contextTypes)
        {
            return _unitOfWorkDictionary.Where(unitOfWork => contextTypes.Contains(unitOfWork.Key)).Sum(unitOfWork => unitOfWork.Value.Commit());
        }

        public async Task<int> CommitMultipleAsync(params Type[] contextTypes)
        {
            var count = 0;
            foreach (var unitOfWork in _unitOfWorkDictionary)
            {
                if (contextTypes.Contains(unitOfWork.Key))
                    count += await unitOfWork.Value.CommitAsync();
            }
            return count;
        }

        public int CommitAll()
        {
            return _unitOfWorkDictionary.Sum(unitOfWorkItem => unitOfWorkItem.Value.Commit());
        }

        public async Task<int> CommitAllAsync()
        {
            var count = 0;
            foreach (var unitOfWorkItem in _unitOfWorkDictionary)
            {
                count += await unitOfWorkItem.Value.CommitAsync();
            }
            return count;
        }

        public void Dispose()
        {
            foreach (var keyValue in _unitOfWorkDictionary)
            {
                keyValue.Value.Dispose();
                _unitOfWorkDictionary.Remove(keyValue.Key);
            }
            GC.SuppressFinalize(this);
        }
    }
}
