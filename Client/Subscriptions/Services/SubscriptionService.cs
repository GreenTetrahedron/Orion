using Orion.Client.Subscriptions;
using Orion.Models.ServerTransmissions.Results;

namespace Orion.Client.Subscriptions.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly HashSet<string> _topics;
        private readonly Dictionary<string, Type> _topicToOperationMessageCodeType;
        private readonly Dictionary<string, Action<object?>> _topicToHandler;
        private readonly Dictionary<string, Subscriptable<Enum>> _topicToSubscriptable;

        public SubscriptionService()
        {
            _topics = new HashSet<string>();
            _topicToOperationMessageCodeType = new Dictionary<string, Type>();
            _topicToHandler = new Dictionary<string, Action<object?>>();
            _topicToSubscriptable = new Dictionary<string, Subscriptable<Enum>>();
        }

        public Subscriptable<T> GetOrCreateSubscriptableForTopic<T>(string topic) where T : Enum
        {
            if(_topics.Contains(topic))
                return _topicToSubscriptable[topic];

            var subscriptable = new Subscriptable<T>();

            _topics.Add(topic);
            _topicToOperationMessageCodeType[topic] = typeof(T);
            _topicToSubscriptable[topic] = subscriptable;
            _topicToHandler[topic] = data =>
            {
                ServerResult baseResult = (ServerResult)data;
                ServerResult<T> result = (ServerResult<T>)baseResult;
                subscriptable.Publish(result);
            };

            return subscriptable;
        }

        public bool TryPublishDataForTopic(string topic, object? data)
        {
            if (!_topics.Contains(topic))
                return false;

            _topicToHandler[topic].Invoke(data);
            return true;
        }
    }
}
