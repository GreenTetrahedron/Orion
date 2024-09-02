using Orion.Models.ServerTransmissions;
using Orion.Router.Attributes;
using Orion.Configuration;
using System.Reflection;

namespace Orion.Router.TopicInterceptors
{
    public class TopicInterceptorService : ITopicInterceptorService
    {
        private readonly IDictionary<string, Action<ServerResponse>> topicToInterceptor;

        public TopicInterceptorService(IConfigurationService configurationService)
        {
            topicToInterceptor = new Dictionary<string, Action<ServerResponse>>();


            var controllers =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(ControllerAttribute), false)
                where attributes != null && attributes.Length > 0
                select t;


            foreach (var controller in controllers)
            {
                var constructor = controller.GetConstructors().Single();
                var constructorParameters = constructor.GetParameters();

                var arguments = new List<object>();

                foreach (var parameter in constructorParameters)
                {
                    var argument = configurationService.GetInstanceOfType(parameter.ParameterType);
                    arguments.Add(argument);
                }

                var controllerInstance = constructor.Invoke(arguments.ToArray());

                var interceptors = controller.GetMethods()
                    .Where(m => m.GetCustomAttributes(typeof(InterceptorAttribute), false) != null && m.GetCustomAttributes(typeof(InterceptorAttribute), false).Length > 0);

                foreach (var interceptor in interceptors)
                {
                    topicToInterceptor.Add(interceptor.GetCustomAttribute<InterceptorAttribute>().Topic, async (x) =>
                    {
                        await (Task)interceptor.Invoke(controllerInstance, [x]);
                    });
                }
            }
        }

        public bool TryGetTopicInterceptor(string topic, out Action<ServerResponse>? interceptor)
        {
            var result = topicToInterceptor.TryGetValue(topic, out interceptor);

            return result;
        }
    }
}
