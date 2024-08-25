using Orion.Client.Subscriptions;
using Orion.Client.Subscriptions.Services;
using Orion.Client.Tests.Mocks;
using Orion.Client.Tests.Tests.Transmissions;

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
    }
}
