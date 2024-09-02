using Orion.Client.Attributes;
using Orion.Configuration;
using System.Reflection;

namespace Orion.Client.TopicHandlers
{
    public class TopicHandlerService : ITopicHandlerService
    {
        private readonly IDictionary<string, Action<object>> topicToHandler;

        public TopicHandlerService(IConfigurationService configurationService)
        {
            topicToHandler = new Dictionary<string, Action<object>>();


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

                var handlers = controller.GetMethods()
                    .Where(m => m.GetCustomAttributes(typeof(HandlerAttribute), false) != null && m.GetCustomAttributes(typeof(HandlerAttribute), false).Length > 0);

                foreach (var handler in handlers)
                {
                    topicToHandler.Add(handler.GetCustomAttribute<HandlerAttribute>().Topic, async (x) =>
                    {
                        await (Task)handler.Invoke(controllerInstance, [x]);
                    });
                }
            }
        }

        public Action<object>? GetTopicHandler(string topic)
        {
            topicToHandler.TryGetValue(topic, out var handler);

            return handler;
        }
    }
}
