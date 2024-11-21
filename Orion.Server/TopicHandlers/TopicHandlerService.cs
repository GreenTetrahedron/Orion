using Orion.Server.Attributes;
using Orion.Configuration;
using System.Reflection;
using Orion.Models.ServerTransmissions;
using Newtonsoft.Json.Linq;

namespace Orion.Server.TopicHandlers
{
    public class TopicHandlerService : ITopicHandlerService
    {
        private readonly IDictionary<string, Type> topicToHandlerControllerType;

        private readonly IConfigurationService _configurationService;

        public TopicHandlerService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;

            topicToHandlerControllerType = new Dictionary<string, Type>();


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
                    var argument = configurationService.GetSingletonOfType(parameter.ParameterType) ?? configurationService.GetScopedOfType(parameter.ParameterType);
                    arguments.Add(argument);
                }

                var controllerInstance = constructor.Invoke(arguments.ToArray());

                var handlers = controller.GetMethods()
                    .Where(m => m.GetCustomAttributes(typeof(HandlerAttribute), false) != null && m.GetCustomAttributes(typeof(HandlerAttribute), false).Length > 0);

                foreach (var handler in handlers)
                {
                    topicToHandlerControllerType.Add(handler.GetCustomAttribute<HandlerAttribute>().Topic, controller);
                }
            }
        }

        public Func<object, Task<ServerTransmission>>? GetTopicHandler(string topic)
        {
            _configurationService.ResetScope();

            bool topicIsKnown = topicToHandlerControllerType.ContainsKey(topic);

            if (!topicIsKnown)
                throw new Exception($"Unknown topic: {topic})");

            var constructorInfo = topicToHandlerControllerType[topic].GetConstructors().Single();

            var parameters = constructorInfo.GetParameters();
            var arguments = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                arguments[i] = _configurationService.GetInstanceOfType(parameters[i].ParameterType);
            }

            var controllerInstance = constructorInfo.Invoke(arguments);

            var handler = topicToHandlerControllerType[topic].GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(HandlerAttribute), false) != null && m.GetCustomAttributes(typeof(HandlerAttribute), false).Length > 0)
                .Where(m => m.GetCustomAttribute<HandlerAttribute>().Topic == topic)
                .Single();


            return async (x) =>
            {
                Task handlerTask = (Task)handler.Invoke(controllerInstance, [Convert.ChangeType(x, handler.GetParameters()[0].ParameterType)]);
                await handlerTask.ConfigureAwait(false);
                return (ServerTransmission)((dynamic)handlerTask).Result;
            };
        }
    }
}
