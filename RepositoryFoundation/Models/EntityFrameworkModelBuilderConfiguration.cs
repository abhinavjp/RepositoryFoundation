using System.Configuration;

namespace RepositoryFoundation.Models
{
    public class EntityFrameworkModelBuilderConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("modelConfiguration", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(string), AddItemName = "model")]
        public ModelConfigurationCollection ModelConfigurations => (ModelConfigurationCollection)this["modelConfiguration"];
    }
}
