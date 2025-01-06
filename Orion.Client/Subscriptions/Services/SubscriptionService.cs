using Orion.Models.ServerTransmissions.Results;
using System.Collections.Concurrent;

namespace Orion.Client.Subscriptions.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly HashSet<string> _topics;
        private readonly ConcurrentDictionary<string, Action<object?>> _topicToHandler;
        private readonly ConcurrentDictionary<string, Subscriptable<Enum>> _topicToSubscriptable;

        public SubscriptionService()
        {
            _topics = new HashSet<string>();
            _topicToHandler = new ConcurrentDictionary<string, Action<object?>>();
            _topicToSubscriptable = new ConcurrentDictionary<string, Subscriptable<Enum>>();
        }

        public Subscriptable<T> GetOrCreateSubscriptableForTopic<T>(string topic) where T : Enum
        {
            if (_topics.Contains(topic))
                return _topicToSubscriptable[topic];

            var subscriptable = new Subscriptable<T>();

            _topics.Add(topic);
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
            _topicToHandler.TryRemove(topic, out _);
            _topicToSubscriptable.TryRemove(topic, out _);
            _topics.Remove(topic);

            return true;
        }
    }
}
