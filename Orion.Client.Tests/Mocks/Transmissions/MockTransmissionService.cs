using Orion.Client.Subscriptions;
using Orion.Client.Transmissions;
using Orion.Models.ClientTransmissions;

namespace Orion.Client.Tests.Mocks.Transmissions
{
    public class MockTransmissionService : ITransmissionService
    {
        public readonly MethodCallStack _callStack;

        public MockTransmissionService()
        {
            _callStack = new MethodCallStack();
        }

        public async Task InitialiseRouterConnection()
        {
            _callStack.NewMethodCall(nameof(this.InitialiseRouterConnection));
        }

        public async Task<ClientTransmission?> ReceiveData()
        {
            _callStack.NewMethodCall(nameof(this.ReceiveData));

            return default;
        }

        public async Task<Subscriptable<T>> TransmitDataOfTopic<T>(object? data, string topic) where T : Enum
        {
            _callStack.NewMethodCall("TransmitDataOfTopic", [data, topic]);

            return default;
        }
    }
}
