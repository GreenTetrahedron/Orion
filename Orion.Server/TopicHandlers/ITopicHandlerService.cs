using Orion.Models.ServerTransmissions;

namespace Orion.Server.TopicHandlers
{
    public interface ITopicHandlerService
    {
        Func<object, Task<ServerTransmission>>? GetTopicHandler(string topic);
    }
}
