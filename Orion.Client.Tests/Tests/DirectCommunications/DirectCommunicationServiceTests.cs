using Orion.Client.DirectCommunications.Services;
using Orion.Client.Tests.Mocks.Transmissions;

namespace Orion.Client.Tests.Tests.DirectCommunications
{
    public class DirectCommunicationServiceTests
    {
        private IDirectCommunicationService _directCommunicationService;
        private MockTransmissionService _mockTransmissionService;

        [SetUp]
        public void Setup()
        {
            _mockTransmissionService = new MockTransmissionService();
            _directCommunicationService = new DirectCommunicationService(_mockTransmissionService);
        }
        [Test]
        public async Task NewDirectCommunication_CallsTransmitDataOfTopicWithCorrectGuidsAndTopic()
        {
            var senderId = Guid.NewGuid();
            var receiverName = "Receiver";

            var directCommunication = new NewDirectCommunicationDTO { SenderId = senderId, ReceiverName = receiverName };

            await _directCommunicationService.NewDirectCommunication(directCommunication);

            var lastMethodCall = _mockTransmissionService._callStack.GetLastMethodCall();

            Assert.IsNotNull(lastMethodCall, "No method was called...");

            string nameOfLastMethodCalled = lastMethodCall.Item1;
            object?[] argumentsPassedInLastMethodCall = lastMethodCall.Item2;

            Assert.That(nameOfLastMethodCalled == "TransmitDataOfTopic", "Wrong method called...");
            Assert.That(argumentsPassedInLastMethodCall.SequenceEqual([directCommunication, "NewDirectCommunication"]), "Wrong arguments passed...");
        }
    }
}
