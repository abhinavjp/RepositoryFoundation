using System.Configuration;

namespace RepositoryFoundation.Models
{
    public class ModelConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key => (string)this["key"];
        [ConfigurationProperty("name")]
        public string ModelName => (string)this["name"];
        [ConfigurationProperty("provider")]
        public string ProviderName => string.IsNullOrWhiteSpace((string) this["provider"]) ? "System.Data.SqlClient" : (string)this["provider"];
        [ConfigurationProperty("connectionStringName", IsRequired = true)]
        public string ConnectionStringName => (string)this["connectionStringName"];
    }
}