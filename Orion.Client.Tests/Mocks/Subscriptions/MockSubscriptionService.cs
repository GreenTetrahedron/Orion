using Orion.Client.Subscriptions;
using Orion.Client.Subscriptions.Services;

namespace Orion.Client.Tests.Mocks.Subscriptions
{
    public class MockSubscriptionService : ISubscriptionService
    {
        public MethodCallStack callStack;

        public MockSubscriptionService()
        {
            callStack = new MethodCallStack();
        }

        public Subscriptable<T> GetOrCreateSubscriptableForTopic<T>(string topic) where T : Enum
        {
            callStack.NewMethodCall("GetOrCreateSubscriptableForTopic", [topic]);

            return default;
        }

        public bool TryPublishDataForTopic(string topic, object? data)
        {
            callStack.NewMethodCall(nameof(this.TryPublishDataForTopic), [topic, data]);

            return default;
        }
    }
}
