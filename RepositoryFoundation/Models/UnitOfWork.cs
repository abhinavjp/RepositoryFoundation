using RepositoryFoundation.Interfaces;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading.Tasks;
using static RepositoryFoundation.Repository.Infrastructure.StructureMapConfigurator;

namespace RepositoryFoundation.Repository.Models
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        private readonly TContext _context;
        private const string SectionName = "repositoryFoundation";

        public UnitOfWork(string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelName))
            {
                _context = new TContext();
            }
            var efModelBuilderConfiguration = ConfigurationManager.GetSection(SectionName) as EntityFrameworkModelBuilderConfiguration;
            if (efModelBuilderConfiguration == null)
            {
                Debug.WriteLine("Section 'repositoryFoundation' not found in web.config. Reverting to fallback context");
                _context = new TContext();
            }
            else
            {
                var modelConfiguration = efModelBuilderConfiguration.ModelConfigurations[modelName];
                var nameOrConnectionString = EntityFrameworkConnectionBuilder.CreateEntityFrameworkConnection(modelConfiguration.ModelName,
                    modelConfiguration.ProviderName, ConfigurationManager.ConnectionStrings[modelConfiguration.ConnectionStringName]?.ToString());
                _context = Activator.CreateInstance(typeof(TContext), nameOrConnectionString) as TContext;
            }
        }

        public UnitOfWork():this(string.Empty)
        {
        }

        public IGenericRepository<TContext, TEntity, TIdType> GetRepository<TEntity, TIdType>(Func<TEntity, TIdType> idGetter) where TEntity : class where TIdType : struct
        {
            return GetInstance<IGenericRepository<TContext, TEntity, TIdType>>(_context, idGetter);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int CommitMultiple(params IUnitOfWork[] unitOfWorks)
        {
            var count = 0;
            foreach (var unitOfWork in unitOfWorks)
            {
                count += unitOfWork.Commit();
            }
            return count;
        }

        public async Task<int> CommitMultipleAsync(params IUnitOfWork[] unitOfWorks)
        {
            var count = 0;
            foreach (var unitOfWork in unitOfWorks)
            {
                count += await unitOfWork.CommitAsync();
            }
            return count;
        }

        public virtual void SetLogger(Action<string> logger)
        {
            _context.Database.Log = logger;
        }

        public void SetCommandTimeout(int timeOut)
        {
            _context.Database.CommandTimeout = timeOut;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}