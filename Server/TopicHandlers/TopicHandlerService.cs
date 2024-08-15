using Orion.Models.RouterTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.Attributes;
using Orion.Server.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.TopicHandlers
{
    public class TopicHandlerService : ITopicHandlerService
    {
        private readonly IDictionary<string, Func<object, Task<ServerResult>>> topicToHandler;

        public TopicHandlerService(IConfigurationService configurationService)
        {
            topicToHandler = new Dictionary<string, Func<object, Task<ServerResult>>>();


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
                        return await ((Task<ServerResult>)handler.Invoke(controllerInstance, new object[1] { x }));
                    });
                }
            }
        }

        public Func<object, Task<ServerResult>>? GetTopicHandler(string topic)
        {
            topicToHandler.TryGetValue(topic, out var handler);

            return handler;
        }
    }
}
