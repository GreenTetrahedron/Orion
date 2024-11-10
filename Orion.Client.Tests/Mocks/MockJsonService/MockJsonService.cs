using Orion.JsonParser;

namespace Orion.Client.Tests.Mocks.MockJsonService
{
    public class MockJsonService : IJsonService
    {
        public readonly MethodCallStack _callStack;

        public MockJsonService()
        {
            _callStack = new MethodCallStack();
        }

        public T? DeserialiseJson<T>(string json)
        {
            _callStack.NewMethodCall("DeserialiseJson", [json]);
            return default;
        }

        public string SerialiseObject(object objectToSerialise)
        {
            _callStack.NewMethodCall("SerialiseObject", [objectToSerialise]);

            return "";
        }
    }
}
