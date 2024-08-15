namespace Orion.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private IDictionary<Type, object> instances;

        public ConfigurationService()
        {
            instances = new Dictionary<Type, object>();
        }

        public void AddInstanceOfType<T>(T instance)
        {
            instances.Add(typeof(T), instance);
        }

        public T? GetInstanceOfType<T>()
        {
            instances.TryGetValue(typeof(T), out var instance);

            return (T?)instance;
        }


        public object GetInstanceOfType(Type type)
        {
            instances.TryGetValue(type, out var instance);

            return instance;
        }
    }
}
