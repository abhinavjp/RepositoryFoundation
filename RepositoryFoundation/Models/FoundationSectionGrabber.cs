using System.Configuration;
using System.Data;

namespace RepositoryFoundation.Models
{
    public class FoundationSectionGrabber
    {
        private const string SectionName = "repositoryFoundation";

        public static string GetConnectionString(ModelConfigurationElement modelConfiguration)
        {
            if (modelConfiguration == null) return null;
            return EntityFrameworkConnectionBuilder.CreateEntityFrameworkConnection(modelConfiguration.ProviderName,
                ConfigurationManager.ConnectionStrings[modelConfiguration.ConnectionStringName]?.ToString(), modelConfiguration.ModelName);
        }

        public static ModelConfigurationElement GetModelConfiguration(string entityName)
        {
            var section = GetConfigurationSection();
            return section?.ModelConfigurations[entityName];
        }

        internal static EntityFrameworkModelBuilderConfiguration GetConfigurationSection()
        {
            return GetConfigurationSection<EntityFrameworkModelBuilderConfiguration>(SectionName);
        }

        public static TSection GetConfigurationSection<TSection>(string sectionName)
        {
            if (!(ConfigurationManager.GetSection(sectionName) is TSection section))
                return default(TSection);
            return section;
        }
    }
}
