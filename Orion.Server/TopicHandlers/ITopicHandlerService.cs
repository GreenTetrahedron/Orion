using Orion.Models.ServerTransmissions;
using Orion.Models.UserModels;

namespace Orion.Server.TopicHandlers
{
    public interface ITopicHandlerService
    {
        Func<object, Task<ServerTransmission>>? GetTopicHandler(string topic);

        Roles GetAuthorisedRoleByTopic(string topic);
    }
}
