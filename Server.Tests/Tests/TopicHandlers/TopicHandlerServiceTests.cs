using Orion.Configuration;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.Tests.Mocks;
using Orion.Server.Tests.Mocks.Users.Controllers;
using Orion.Server.TopicHandlers;
using Orion.Server.Users.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Tests.TopicHandlers
{
    public class TopicHandlerServiceTests
    {
        private ITopicHandlerService _topicHandlerService;
        private MockMethodCallStackService _mockMethodCallStackService;
        private IConfigurationService _configurationService;

        [SetUp]
        public void Setup()
        {
            _configurationService = new ConfigurationService();
            _configurationService.AddInstanceOfType<MockMethodCallStackService>(new MockMethodCallStackService());

            _mockMethodCallStackService = _configurationService.GetInstanceOfType<MockMethodCallStackService>();

            _topicHandlerService = new TopicHandlerService(_configurationService);
        }

        [Test]
        [TestCase("User1")]
        [TestCase("User2")]
        [TestCase("User3")]
        public async Task AuthenticateUserIsCalledProperly(string username)
        {
            ServerResult? result = await _topicHandlerService.GetTopicHandler("AuthenticateUser").Invoke(new Credentials() { Username = username });

            bool aMethodWasCalled = _mockMethodCallStackService.MethodCallStack.TryPeek(out string lastMethodCall);

            Assert.That(aMethodWasCalled, "No method called... ");

            string[] lastMethodCallSplit = lastMethodCall.Split(": ");
            string nameOfLastMethodCalled = lastMethodCallSplit[0];
            string lastMethodCallArguments = lastMethodCallSplit[1];

            Assert.That(nameOfLastMethodCalled == "AuthenticateUser", "Wrong method called... ");
            Assert.That(lastMethodCallArguments == username, "Wrong arguments passed... ");
        }
    }
}
