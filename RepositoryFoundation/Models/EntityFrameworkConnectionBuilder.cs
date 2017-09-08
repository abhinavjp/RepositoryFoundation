using RepositoryFoundation.Interfaces;
using System.Data.Entity.Core.EntityClient;

namespace RepositoryFoundation.Repository.Models
{
    public static class EntityFrameworkConnectionBuilder
    {
        public static string CreateEntityFrameworkConnection(string modelName, string providerName, string dbConnectionstring)
        {
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
