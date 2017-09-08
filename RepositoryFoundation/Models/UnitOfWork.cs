using RepositoryFoundation.Interfaces;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Threading.Tasks;
using static RepositoryFoundation.Repository.Infrastructure.StructureMapConfigurator;

namespace RepositoryFoundation.Repository.Models
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        private readonly TContext _context;
        private const string SectionName = "efModelBuilder";
        private const string DefaultConnectionStringName = "efConnectionString";

        public UnitOfWork(string modelName, string connectionStringName)
        {
            var efModelBuilderConfiguration = ConfigurationManager.GetSection(SectionName) as EntityFrameworkModelBuilderConfiguration;
            if (efModelBuilderConfiguration == null)
            {
                _context = new TContext();
            }
            else
            {
                var modelConfiguration = efModelBuilderConfiguration.ModelConfigurations[modelName];
                var nameOrConnectionString = EntityFrameworkConnectionBuilder.CreateEntityFrameworkConnection(modelConfiguration.ModelName,
                    modelConfiguration.ProviderName, ConfigurationManager.ConnectionStrings[connectionStringName]?.ToString());
                _context = Activator.CreateInstance(typeof(TContext), nameOrConnectionString) as TContext;
            }
        }

        public UnitOfWork(string modelName) :
            this(modelName, ConfigurationManager.ConnectionStrings[DefaultConnectionStringName]?.ToString())
        {

        }

        public UnitOfWork()
        {
            _context = new TContext();
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