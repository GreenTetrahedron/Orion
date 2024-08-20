using Orion.Server.Tests.Mocks.Users.Repositories;
using Orion.Server.Users.Controllers;

namespace Orion.Server.Tests.Users.Controllers
{
    public class UserControllerTests
    {
        private IUserController _userController;
        private MockUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new MockUserRepository();
            _userController = new UserController(_userRepository);
        }

        [Test]
        [TestCase("Jhonny Felon")]
        [TestCase("User1")]
        [TestCase("Tim OB. Simon")]
        public async Task AuthenticateUser_CallsAuthenticateUserWithCorrectUsername(string username)
        {
            await _userController.AuthenticateUser(username);

            _userRepository.methodCallStack.TryPeek(out string lastMethodCall);

            Assert.That(!string.IsNullOrEmpty(lastMethodCall), "Method not called");

            var lastMethodCallSplit = lastMethodCall.Split(": ");

            Assert.That(lastMethodCallSplit[0] == "AuthenticateUser", "Wrong method called");
            Assert.That(lastMethodCallSplit[1] == username, "Wrong parameter given");
        }

        [Test]
        public async Task AddDirectCommunication_CallsAddDirectCommunicationWithCorrectParameters()
        {
            var senderId = Guid.NewGuid();
            var receiverId = Guid.NewGuid();

            await _userController.AddDirectCommunication(senderId, receiverId);

            _userRepository.methodCallStack.TryPeek(out string lastMethodCall);

            Assert.That(!string.IsNullOrEmpty(lastMethodCall), "Method not called");

            var lastMethodCallSplit = lastMethodCall.Split(": ");
            var arguments = lastMethodCallSplit[1].Split(", ");

            Assert.That(lastMethodCallSplit[0] == "AddDirectCommunication", "Wrong method called");
            Assert.That(arguments[0] == senderId.ToString() && arguments[1] == receiverId.ToString(), "Wrong parameters given");
        }
    }
}
