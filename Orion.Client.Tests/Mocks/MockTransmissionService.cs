using Orion.Client.Subscriptables;
using Orion.Client.Transmissions;
using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.Tests.Mocks
{
    public class MockTransmissionService : ITransmissionService
    {
        private Stack<Tuple<string, object?[]>> _methodCallStack;

        public MockTransmissionService()
        {
            _methodCallStack = new Stack<Tuple<string, object?[]>>();
        }

        public async Task<Subscriptable<T>> TransmitDataOfTopic<T>(object? data, string topic) where T : Enum
        {
            _methodCallStack.Push(new("TransmitDataOfTopic", [data, topic]));
            return new Subscriptable<T>();
        }

        public Tuple<string, object?[]> GetLastMethodCall()
        {
            return _methodCallStack.Peek();
        }
    }
}
