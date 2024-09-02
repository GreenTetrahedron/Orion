using Orion.Configuration;
using Orion.Models.ServerTransmissions;
using Orion.Models.UserModels;
using Orion.Server.Tests.Mocks;
using Orion.Server.TopicHandlers;

namespace Orion.Server.Tests.TopicHandlers
{
    public class TopicHandlerServiceTests
    {
        private ITopicHandlerService _topicHandlerService;
        private MethodCallStack callStack;
        private IConfigurationService _configurationService;

        [SetUp]
        public void Setup()
        {
            _configurationService = new ConfigurationService();
            _configurationService.AddInstanceOfType<MethodCallStack>(new MethodCallStack());

            callStack = _configurationService.GetInstanceOfType<MethodCallStack>();

            _topicHandlerService = new TopicHandlerService(_configurationService);
        }

        [Test]
        [TestCase("User1")]
        [TestCase("User2")]
        [TestCase("User3")]
        public async Task AuthenticateUserIsCalledProperly(string username)
        {
            ServerTransmission? result = await _topicHandlerService.GetTopicHandler("AuthenticateUser").Invoke(new Credentials() { Username = username });

            callStack.GetLastMethodCall(out var nameOfLastMethodCalled, out var lastMethodCallArguments);

            Assert.That(nameOfLastMethodCalled == "AuthenticateUser", "Wrong method called... ");
            Assert.That(lastMethodCallArguments.Length == 1 && (lastMethodCallArguments[0] as Credentials).Username == username, "Wrong arguments passed... ");
        }
    }
}
