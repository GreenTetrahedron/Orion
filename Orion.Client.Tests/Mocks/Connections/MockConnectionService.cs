using Orion.Client.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.Tests.Mocks.Connections
{
    public class MockConnectionService : IConnectionService
    {
        public readonly MethodCallStack _callStack;

        public MockConnectionService()
        {
            _callStack = new MethodCallStack();
        }

        public Task<bool> SendMessage(byte[] data)
        {
            _callStack.NewMethodCall("SendMessage", [ data ]);

            return default;
        }
    }
}
