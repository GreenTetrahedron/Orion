namespace Orion.Client.Tests.Mocks
{
    public class MethodCallStack
    {
        private readonly Stack<Tuple<string, object?[]?>> _callStack;

        public MethodCallStack()
        {
            _callStack = new Stack<Tuple<string, object?[]?>>();
        }

        public void NewMethodCall(string methodName, object?[]? arguments = null)
        {
            _callStack.Push(new(methodName, arguments));
        }

        public string GetNameOfLastMethodCalled()
        {
            return _callStack.Peek().Item1;
        }

        public object?[]? GetLastMethodCallArguments()
        {
            return _callStack.Peek().Item2;
        }

        public Tuple<string, object?[]?> GetLastMethodCall()
        {
            return _callStack.Peek();
        }

        public void GetLastMethodCall(out string methodName, out object?[]? arguments)
        {
            var lastMethodCall = _callStack.Peek();

            methodName = lastMethodCall.Item1;
            arguments = lastMethodCall.Item2;
        }
    }
}
