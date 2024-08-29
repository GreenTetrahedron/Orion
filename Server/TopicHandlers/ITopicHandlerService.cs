using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;

namespace Orion.Server.TopicHandlers
{
    public interface ITopicHandlerService
    {
        Func<object, Task<ServerTransmission>>? GetTopicHandler(string topic);
    }
}
