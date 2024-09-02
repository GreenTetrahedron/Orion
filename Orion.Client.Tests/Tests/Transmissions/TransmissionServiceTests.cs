using Orion.Client.Tests.Mocks.Connections;
using Orion.Client.Tests.Mocks.MockJsonService;
using Orion.Client.Tests.Mocks.Subscriptions;
using Orion.Client.Transmissions;
using Orion.Models.ClientTransmissions;

namespace Orion.Client.Tests.Tests.Transmissions
{
    public class TransmissionServiceTests
    {
        private MockConnectionService _mockConnectionService;

        private MockJsonService _mockJsonService;

        private MockSubscriptionService _mockSubscriptionService;

        private ITransmissionService _transmissionService;

        [SetUp]
        public void Setup()
        {
            _mockSubscriptionService = new MockSubscriptionService();
            _mockJsonService = new MockJsonService();
            _mockConnectionService = new MockConnectionService();
            _transmissionService = new TransmissionService(_mockConnectionService, _mockJsonService, _mockSubscriptionService);
        }

        [Test]
        [TestCase("Foo")]
        public void TransmitDataOfTopic_CallsSerialiseJsonOnce(string topic)
        {
            _transmissionService.TransmitDataOfTopic<Enum>(null, topic);

            Assert.That(_mockJsonService._callStack.GetNameOfLastMethodCalled() == "SerialiseObject", "SerialiseObject not called...");
        }

        [Test]
        [TestCase("Foo", null)]
        public void TransmitDataOfTopic_ConvertsDataAndTopicToTransmissionObject(string topic, object? data)
        {
            _transmissionService.TransmitDataOfTopic<Enum>(data, topic);

            var expectedTransmission = new ClientTransmission(topic, data);

            var methodCallArguments = _mockJsonService._callStack.GetLastMethodCallArguments();

            Assert.That(methodCallArguments.Length == 1, "Wrong use of method...");

            var actualTransmission = methodCallArguments[0] as ClientTransmission;

            Assert.That(actualTransmission.Data == expectedTransmission.Data
                && actualTransmission.Topic == expectedTransmission.Topic,
                "Did not convert to correct transmission object...");
        }

        [Test]
        [TestCase("FOoo")]
        public async Task TransmitDataOfTopic_CallsSendMessageOnce(string topic)
        {
            await _transmissionService.TransmitDataOfTopic<Enum>(null, topic);

            Assert.That(_mockConnectionService._callStack.GetNameOfLastMethodCalled() == "SendMessage");
        }

        [Test]
        [TestCase("Topic1")]
        public async Task TransmitDataOfTopic_CallsGetOrCreateSubscriptableForTopicWithCorrectTopic(string topic)
        {
            await _transmissionService.TransmitDataOfTopic<Enum>(null, topic);

            _mockSubscriptionService.callStack.GetLastMethodCall(out var methodName, out var arguments);

            Assert.IsNotNull(methodName, "No method called...");

            Assert.That(methodName == "GetOrCreateSubscriptableForTopic", "Wrong method called");
            Assert.That(arguments.SequenceEqual([topic + "Result"]), "Wrong topic passed...");
        }
    }
}
