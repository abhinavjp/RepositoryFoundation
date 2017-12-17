using System.Configuration;

namespace RepositoryFoundation.Models
{
    public class ModelConfigurationCollection : ConfigurationElementCollection
    {

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.AddRemoveClearMap;

        protected override ConfigurationElement CreateNewElement() => new ModelConfigurationElement();

        protected override object GetElementKey(ConfigurationElement element) => ((ModelConfigurationElement)element).Key;

        public ModelConfigurationElement this[int index]
        {
            get => (ModelConfigurationElement)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new ModelConfigurationElement this[string name] => (ModelConfigurationElement)BaseGet(name);

        public int IndexOf(ModelConfigurationElement modelconfiguration) => BaseIndexOf(modelconfiguration);

        public void Add(ModelConfigurationElement modelconfiguration)
        {
            BaseAdd(modelconfiguration);

        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ModelConfigurationElement modelconfiguration)
        {
            if (BaseIndexOf(modelconfiguration) >= 0)
            {
                BaseRemove(modelconfiguration.Key);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}