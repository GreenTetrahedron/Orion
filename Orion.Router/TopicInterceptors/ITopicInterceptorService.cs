using Orion.Models.ServerTransmissions;

namespace Orion.Router.TopicInterceptors
{
    public interface ITopicInterceptorService
    {
        bool TryGetTopicInterceptor(string topic, out Action<ServerResponse> handler);
    }
}
