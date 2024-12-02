namespace Orion.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private IDictionary<Type, object> singletons;
        
        private List<Type> scopedTypesOrder;
        private IDictionary<Type, Type> scopedTypes;
        private IDictionary<Type, object> scopedInstances;


        public ConfigurationService()
        {
            singletons = new Dictionary<Type, object>();
            scopedTypes = new Dictionary<Type, Type>();
            scopedInstances = new Dictionary<Type, object>();
            scopedTypesOrder = new List<Type>();
        }

        public void AddScoped<Type, InstanceType>() where InstanceType : Type
        {
            scopedTypesOrder.Add(typeof(Type));
            scopedTypes.Add(typeof(Type), typeof(InstanceType));

            var constructors = typeof(InstanceType).GetConstructors();
            var constructorInfo = constructors.First();

            var parameters = constructorInfo.GetParameters();
            object[] arguments = new object[parameters.Length];

            for (int i = 0; i < arguments.Length; i++)
            {
                arguments[i] = GetScopedOfType(parameters[i].ParameterType) ?? GetSingletonOfType(parameters[i].ParameterType);
            }

            scopedInstances.Add(typeof(Type), constructorInfo.Invoke(arguments));
        }

        public void AddSingleton<Type, InstanceType>() where InstanceType : Type
        {
            var constructorInfo = typeof(InstanceType).GetConstructors().Single();

            var parameters = constructorInfo.GetParameters();
            object[] arguments = new object[parameters.Length];

            for(int i = 0; i < arguments.Length; i++)
            {
                arguments[i] = GetSingletonOfType(parameters[i].ParameterType);
            }

            singletons.Add(typeof(Type), constructorInfo.Invoke(arguments));
        }

        public void AddSingleton<T>(T instance)
        {
            singletons.Add(typeof(T), instance);
        }

        public T GetInstanceOfType<T>()
        {
            return (T)GetInstanceOfType(typeof(T));
        }

        public object GetInstanceOfType(Type type)
        {
            return GetScopedOfType(type) ?? GetSingletonOfType(type);
        }

        public T GetScopedOfType<T>()
        {
            //scopedInstances.TryGetValue(typeof(T), out var instance);

            //return (T?)instance;

            return (T)GetScopedOfType(typeof(T));
        }

        public object GetScopedOfType(Type type)
        {
            if (!scopedInstances.ContainsKey(type))
                return null;

            return scopedInstances[type];
        }

        public T? GetSingletonOfType<T>()
        {
            singletons.TryGetValue(typeof(T), out var instance);

            return (T?)instance;
        }


        public object GetSingletonOfType(Type type)
        {
            singletons.TryGetValue(type, out var instance);

            return instance;
        }

        public void ResetScope()
        {
            foreach(var type in scopedTypesOrder)
            {
                var obj = GetScopedOfType(type);
                
                var constructorInfo = scopedTypes[type].GetConstructors().First();

                var parameters = constructorInfo.GetParameters();
                object[] arguments = new object[parameters.Length];

                for (int i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = GetScopedOfType(parameters[i].ParameterType) ?? GetSingletonOfType(parameters[i].ParameterType);
                }

                scopedInstances[type] = constructorInfo.Invoke(arguments);
                
                if (obj is IDisposable)
                {
                    ((IDisposable)obj).Dispose();
                }
            }
        }
    }
}
