namespace Orion.Router.Configuration
{
    public interface IConfigurationService
    {
        T GetInstanceOfType<T>();

        object GetInstanceOfType(Type type);

        void AddInstanceOfType<T>(T instance);
    }
}
