using Orion.Client.Connections;
using Orion.Client.Transmissions;

namespace Orion.Client.Tests.Mocks.Connections
{
    public class MockConnectionService : IConnectionService
    {
        public readonly MethodCallStack _callStack;

        public MockConnectionService()
        {
            _callStack = new MethodCallStack();
        }

        public Task<MessageBytes> ReceiveMessage()
        {
            _callStack.NewMethodCall(nameof(this.ReceiveMessage));

            return default;
        }

        public Task<bool> SendMessage(byte[] data)
        {
            _callStack.NewMethodCall("SendMessage", [data]);

            return default;
        }
    }
}
