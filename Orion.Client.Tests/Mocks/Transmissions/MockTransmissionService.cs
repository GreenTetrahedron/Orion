using Orion.Client.Subscriptions;
using Orion.Client.Transmissions;
using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.Tests.Mocks.Transmissions
{
    public class MockTransmissionService : ITransmissionService
    {
        public readonly MethodCallStack _callStack;

        public MockTransmissionService()
        {
            _callStack = new MethodCallStack();
        }

        public async Task<Subscriptable<T>> TransmitDataOfTopic<T>(object? data, string topic) where T : Enum
        {
            _callStack.NewMethodCall("TransmitDataOfTopic", [data, topic]);
            return default;
        }
    }
}
