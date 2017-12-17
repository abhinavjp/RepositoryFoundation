using System.Data.Entity.Core.EntityClient;

namespace RepositoryFoundation.Models
{
    public static class EntityFrameworkConnectionBuilder
    {
        public static string CreateEntityFrameworkConnection(string providerName, string dbConnectionstring)
        {
            return CreateEntityFrameworkConnection(providerName, dbConnectionstring, string.Empty);
        }
        public static string CreateEntityFrameworkConnection(string providerName, string dbConnectionstring, string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelName))
                return dbConnectionstring;
            var entityConnectionStringBuilder = new EntityConnectionStringBuilder
            {
                Provider = providerName,
                ProviderConnectionString = dbConnectionstring,
                Metadata = $@"res://*/{modelName}.csdl|res://*/{modelName}.ssdl|res://*/{modelName}.msl"
            };

            return entityConnectionStringBuilder.ToString();
        }
    }
}
