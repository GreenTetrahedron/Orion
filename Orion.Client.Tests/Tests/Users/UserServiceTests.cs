using Orion.Client.Subscriptions;
using Orion.Client.Tests.Mocks.Transmissions;
using Orion.Client.Users.Services;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using System.Threading.Tasks.Sources;

namespace Orion.Client.Tests.Tests.Users
{
    public class Tests
    {
        private IUserService _userService;
        private MockTransmissionService _mockTransmissionService;


        [SetUp]
        public void Setup()
        {
            _mockTransmissionService = new MockTransmissionService();
            _userService = new UserService(_mockTransmissionService);
        }

        [Test]
        [TestCase("User1")]
        public async Task AuthenticateUser_CallsCorrectTransmissionServiceMethod(string username)
        {
            var credentials = new Credentials { Username = username };

            await _userService.AuthenticateUser(credentials);

            var lastMethodCall = _mockTransmissionService._callStack.GetLastMethodCall();

            Assert.IsNotNull(lastMethodCall, "No method was called...");

            string nameOfLastMethodCalled = lastMethodCall.Item1;
            object?[] argumentsPassedInLastMethodCall = lastMethodCall.Item2;

            Assert.That(nameOfLastMethodCalled == "TransmitDataOfTopic", "Wrong method called...");
            Assert.That(argumentsPassedInLastMethodCall.SequenceEqual([credentials, "AuthenticateUser"]), "Wrong arguments passed...");
        }
    }
}