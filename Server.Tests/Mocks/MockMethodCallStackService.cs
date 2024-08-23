using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Tests.Mocks
{
    public class MockMethodCallStackService
    {
        public Stack<string> MethodCallStack { get; private set; }

        public MockMethodCallStackService()
        {
            MethodCallStack = new Stack<string>();
        }
    }
}
