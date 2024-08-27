namespace Orion.Client.TopicHandlers
{
    public interface ITopicHandlerService
    {
        Action<object>? GetTopicHandler(string topic);
    }
}
