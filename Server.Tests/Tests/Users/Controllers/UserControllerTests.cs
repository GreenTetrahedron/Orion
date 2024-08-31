using Orion.Models.ClientTransmissions;
using Orion.Models.UserModels;
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
            var credentials = new Credentials() { Username = username };

            await _userController.AuthenticateUser(credentials);

            _userRepository.methodCallStack.TryPeek(out string lastMethodCall);

            Assert.That(!string.IsNullOrEmpty(lastMethodCall), "Method not called");

            var lastMethodCallSplit = lastMethodCall.Split(": ");

            Assert.That(lastMethodCallSplit[0] == "AuthenticateUser", "Wrong method called");
            Assert.That(lastMethodCallSplit[1] == credentials.Username, "Wrong parameter given");
        }

        [Test]
        [TestCase("Jhonny Felon")]
        [TestCase("User1")]
        [TestCase("Tim OB. Simon")]
        public async Task GetUserByUsername_CallsGetUserProfileByUsernameWithCorrectUsername(string username)
        {
            await _userController.GetUserByUsername(username);

            _userRepository.methodCallStack.TryPeek(out string lastMethodCall);

            Assert.That(!string.IsNullOrEmpty(lastMethodCall), "Method not called");

            var lastMethodCallSplit = lastMethodCall.Split(": ");

            Assert.That(lastMethodCallSplit[0] == nameof(MockUserRepository.GetUserProfileByUsername), "Wrong method called");
            Assert.That(lastMethodCallSplit[1] == username, "Wrong parameter given");
        }
    }
}
