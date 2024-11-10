namespace Orion.Configuration
{
    public interface IConfigurationService
    {
        public T GetSingletonOfType<T>();

        public object GetSingletonOfType(Type type);

        public void AddSingleton<Type, InstanceType>() where InstanceType : Type;
        public void AddSingleton<T>(T instance);

        public T GetScopedOfType<T>();

        public object GetScopedOfType(Type type);

        public void AddScoped<Type, InstanceType>() where InstanceType : Type;

        public T GetInstanceOfType<T>();
        public object GetInstanceOfType(Type type);

        public void ResetScope();
    }
}
