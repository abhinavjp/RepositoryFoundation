using RepositoryFoundation.Interfaces;
using RepositoryFoundation.Models;
using StructureMap;
using StructureMap.Pipeline;

namespace RepositoryFoundation.Infrastructure
{
    public static class UnitOfWorkConfigurator
    {
        private static Container _container;
        private static bool _isInitialized;

        internal static void Configure()
        {
            InitializeContainer(new Container());
        }

        internal static void InitializeContainer(Container externalContainer)
        {
            _container = externalContainer;
            _container.Configure(x =>
            {
                // Repository
                x.For(typeof(IUnitOfWork<>)).Use(typeof(UnitOfWork<>));
                x.For(typeof(IGenericRepository<,,>)).Use(typeof(GenericRepository<,,>));
            });
            _isInitialized = true;
        }

        internal static T GetInstance<T>()
        {
            if (!_isInitialized)
            {
                Configure();
            }
            return _container.GetInstance<T>();
        }

        internal static T GetInstance<T>(ExplicitArguments args)
        {
            if (!_isInitialized)
            {
                Configure();
            }
            return _container.GetInstance<T>(args);
        }

        internal static T GetInstance<T>(params object[] args)
        {
            var explicitArguments = new ExplicitArguments();
            foreach (var arg in args)
            {
                explicitArguments.Set(arg.GetType(), arg);
            }
            if (!_isInitialized)
            {
                Configure();
            }
            return _container.GetInstance<T>(explicitArguments);
        }
    }
}
