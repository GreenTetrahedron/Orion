using Orion.Models.ServerTransmissions.Results;

namespace Orion.Server.TopicHandlers
{
    public interface ITopicHandlerService
    {
        Func<object, Task<ServerResult>>? GetTopicHandler(string topic);
    }
}
