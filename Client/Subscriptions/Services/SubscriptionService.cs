using Orion.Client.Subscriptions;

namespace Orion.Client.Subscriptions.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly Dictionary<string, Subscriptable<Enum>> _subscriptableToTopic;

        public SubscriptionService()
        {
            _subscriptableToTopic = new Dictionary<string, Subscriptable<Enum>>();
        }

        public Subscriptable<T> GetOrCreateSubscriptableForTopic<T>(string topic) where T : Enum
        {
            if(_subscriptableToTopic.TryGetValue(topic, out var subscriptable))
                return subscriptable;

            subscriptable = new Subscriptable<T>();

            _subscriptableToTopic[topic] = subscriptable;

            return subscriptable;
        }
    }
}
