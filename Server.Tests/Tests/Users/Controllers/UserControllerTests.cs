using NUnit.Framework;
using Orion.Server.Tests.Mocks.Users.Repositories;
using Orion.Server.Users.Controllers;
using Orion.Server.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Assert.That(!string.IsNullOrEmpty(lastMethodCall));

            var lastMethodCallSplit = lastMethodCall.Split(": ");

            Assert.That(lastMethodCallSplit[0] == "AuthenticateUser" && lastMethodCallSplit[1] == username);
        }
    }
}
