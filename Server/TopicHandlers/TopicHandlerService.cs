using Orion.Models.ServerTransmissions.Results;
using Orion.Server.Attributes;
using Orion.Configuration;
using System.Reflection;
using Orion.Models.ClientTransmissions;

namespace Orion.Server.TopicHandlers
{
    public class TopicHandlerService : ITopicHandlerService
    {
        private readonly IDictionary<string, Func<object, Task<ServerResult>>> topicToHandler;

        public TopicHandlerService(IConfigurationService configurationService)
        {
            topicToHandler = new Dictionary<string, Func<object, Task<ServerResult>>>();


            var controllers =
                from t in Assembly.GetCallingAssembly().GetTypes()
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
                        Task handlerTask = (Task)handler.Invoke(controllerInstance, [Convert.ChangeType(x, handler.GetParameters()[0].ParameterType)]);
                        await handlerTask.ConfigureAwait(false);
                        return (ServerResult)((dynamic)handlerTask).Result;
                    });
                }
            }
        }

        public Func<object, Task<ServerResult>>? GetTopicHandler(string topic)
        {
            bool topicHadHandler = topicToHandler.TryGetValue(topic, out var handler);

            if (!topicHadHandler)
                throw new Exception($"No handler found for requested topic: {topic})");

            return handler;
        }
    }
}
