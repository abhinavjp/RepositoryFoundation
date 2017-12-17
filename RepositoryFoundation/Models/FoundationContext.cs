using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace RepositoryFoundation.Models
{
    public class FoundationContext : DbContext
    {
        public FoundationContext(string entityNameOrConnectionString) : base(
            GetConnectionString(entityNameOrConnectionString))
        {
        }

        public FoundationContext(string entityNameOrConnectionString, DbCompiledModel model) : base(
            GetConnectionString(entityNameOrConnectionString), model)
        {
        }

        public FoundationContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection,
            contextOwnsConnection)
        {
        }

        public FoundationContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext,
            dbContextOwnsObjectContext)
        {
        }

        public FoundationContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) :
            base(existingConnection, model, contextOwnsConnection)
        {
        }

        protected FoundationContext()
        {
        }

        protected FoundationContext(DbCompiledModel model) : base(model)
        {
        }

        public static string GetConnectionString(string entityNameOrConnectionString)
        {
            return GetConnectionString(entityNameOrConnectionString, false);
        }

        public static string GetConnectionString(string entityNameOrConnectionString, bool isConnectionString)
        {
            if (isConnectionString)
            {
                return entityNameOrConnectionString;
            }
            var entityName = GetEntityName(entityNameOrConnectionString);
            var modelConfiguration = FoundationSectionGrabber.GetModelConfiguration(entityName);
            var connectionString = FoundationSectionGrabber.GetConnectionString(modelConfiguration);
            return connectionString;
        }

        private static string GetEntityName(string entityName)
        {
            return entityName.ToLower().Contains("name=") ? entityName.Substring(5) : entityName;
        }
    }
}