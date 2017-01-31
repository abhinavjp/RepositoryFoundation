using StructureMap;
using StructureMap.Pipeline;
using RepositoryFoundation.Repository.Interface;
using RepositoryFoundation.Repository.Models;

namespace RepositoryFoundation.Repository.Infrastructure
{
    public static class StructureMapConfigurator
    {
        private static Container container;
        private static bool isInitialized = false;

        private static void Configure()
        {
            container = new Container();
            container.Configure(x =>
            {
                // Repository
                x.For(typeof(IUnitOfWork<>)).Use(typeof(UnitOfWork<>));
                x.For(typeof(IGenericRepository<,,>)).Use(typeof(GenericRepository<,,>));
            });
            isInitialized = true;
        }

        public static void InitializeContainer(Container externalContainer)
        {
            container = externalContainer;
            container.Configure(x =>
            {
                // Repository
                x.For(typeof(IUnitOfWork<>)).Use(typeof(UnitOfWork<>));
                x.For(typeof(IGenericRepository<,,>)).Use(typeof(GenericRepository<,,>));
            });
            isInitialized = true;
        }

        public static T GetInstance<T>()
        {
            if (!isInitialized)
            {
                Configure();
            }
            return container.GetInstance<T>();
        }

        public static T GetInstance<T>(ExplicitArguments args)
        {
            if (!isInitialized)
            {
                Configure();
            }
            return container.GetInstance<T>(args);
        }
    }
}
